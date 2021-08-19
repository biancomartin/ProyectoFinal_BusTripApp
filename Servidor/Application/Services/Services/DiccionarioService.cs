using Domain.Diccionario;
using Domain.DTOs;
using Domain.Entities.Dataset;
using Domain.Interfaces;
using Domain.Matriz;
using Helper.Functions;
using Microsoft.Extensions.Logging;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.Services
{
    public class DiccionarioService : IDiccionarioService
    {
        #region Private Fields

        private readonly IRecorridosRepository _recorridosRepository;
        private readonly IDiccionarioRepository _diccionarioRepository;
        private readonly IColectivosService _colectivosService;
        private readonly IFranjaHorariaRepository _franjaHorariaRepository;
        private readonly ILogger<DiccionarioService> _logger;

        #endregion

        #region Constructor

        public DiccionarioService(IRecorridosRepository recorridosRepository, 
                                    IDiccionarioRepository diccionarioRepository, 
                                    IColectivosService colectivosService, 
                                    IFranjaHorariaRepository franjaHorariaRepository, 
                                    ILogger<DiccionarioService> logger)
        {
            _recorridosRepository = recorridosRepository;
            _diccionarioRepository = diccionarioRepository;
            _colectivosService = colectivosService;
            _franjaHorariaRepository = franjaHorariaRepository;
            _logger = logger;
        }

        #endregion

        #region Public Methods

        public DiccionarioComplejo ObtenerDiccionarioComplejoPorFranja(int franjaId, int trayecto, int linea, int? unidadId)
        {
            try
            {
                var recorrido = _recorridosRepository.ObtenerRecorridos().FirstOrDefault(x => trayecto == x.Base && x.Linea == linea);
                var franjaHoraria = _franjaHorariaRepository.ObtenerFranjaHorarias().FirstOrDefault(x => x.Id == franjaId);
                var diccionarios = _diccionarioRepository.ObtenerDiccionarioFiltrado(recorrido.Id, franjaHoraria.Id, unidadId);

                int unidad = unidadId.HasValue ? unidadId.Value : 0;

                DiccionarioComplejo response = new DiccionarioComplejo() { DiccionarioClaves = new List<EntradaDiccionario>() };

                if (diccionarios.Any())
                {
                    foreach (var diccionario in diccionarios)
                    {
                        response.DiccionarioClaves.Add(new EntradaDiccionario()
                        {
                            UnidadId = diccionario.Unidad,
                            PuntoOrigen = diccionario.PuntoOrigen,
                            PuntoDestino = diccionario.PuntoDestino,
                            CantidadDeMuestras = diccionario.CantidadDeMuestras,
                            Segundos = new List<double>() { diccionario.Tiempo }
                        });
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al intentar buscar el diccionario", ex.InnerException);
            }
        }

        public void GenerarDiccionarios()
        {
            var recorridos = _recorridosRepository.ObtenerRecorridos();
            var franjas = _franjaHorariaRepository.ObtenerFranjaHorarias();
            foreach (var franja in franjas)
            {
                foreach (var recorrido in recorridos)
                {
                    CrearDiccionarioComplejoPorFranja(franja, recorrido);
                }
            }
        }

        #endregion

        #region Private Methods

        private void CrearDiccionarioComplejoPorFranja(FranjaHorariaDTO franjaHoraria, RecorridosDTO recorrido)
        {
            var dataset = _colectivosService.ObtenerDatasetColectivosPorHorario(franjaHoraria);
            var diccionarioParcial = CrearDiccionarioComplejo(dataset, recorrido);
            foreach (var dic in diccionarioParcial.DiccionarioClaves)
            {
                _diccionarioRepository.Insertar(new DiccionarioDTO()
                {
                    CantidadDeMuestras = dic.Segundos.Count,
                    Tiempo = dic.Segundos.Sum() / dic.Segundos.Count,
                    PuntoOrigen = dic.PuntoOrigen,
                    PuntoDestino = dic.PuntoDestino,
                    RecorridoId = recorrido.Id,
                    Unidad = dic.UnidadId,
                    FranjaId = franjaHoraria.Id
                });
            }
        }

        private List<CeldaCompleta> GenerarCeldasConTodaInformacion(List<DatasetHistorico> datasetHistoricos, RecorridosDTO recorrido)
        {
            var columnasMatriz = _colectivosService.ObtenerColumnasMatriz();
            var filasMatriz = _colectivosService.ObtenerFilasMatriz();
            var celdas = _colectivosService.GenerarCeldasAPartirDeLineaYTrayecto(recorrido.Linea, recorrido.Base);

            List<CeldaCompleta> listadoCompletoCeldas = new List<CeldaCompleta>();
            int indice;
            foreach (var entrada in datasetHistoricos)
            {
                CeldaCompleta celdaNueva = new CeldaCompleta() { PosicionesPorRecorrido = new Dictionary<int, int>() };
                var celdaAInsertar = new CeldaMatriz()
                {
                    Columna = ObtenerColumnaAsociada(columnasMatriz, entrada.Longitud),
                    Fila = ObtenerFilaAsociada(filasMatriz, entrada.Latitud)
                };

                celdaNueva.Celda = celdaAInsertar;
                celdaNueva.DatasetHistorico = entrada;
                indice = celdas.FindIndex(x => x.Columna == celdaAInsertar.Columna && x.Fila == celdaAInsertar.Fila);
                celdaNueva.PosicionesPorRecorrido.Add(
                    recorrido.Id,
                    indice
                );

                listadoCompletoCeldas.Add(celdaNueva);
            }

            listadoCompletoCeldas = listadoCompletoCeldas.Where(x => !ContieneClaveValor(x.PosicionesPorRecorrido, recorrido.Id, -1)).ToList();

            return listadoCompletoCeldas;
        }

        private List<List<CeldaCompleta>> GenerarListaConCeldasConIndice(List<CeldaCompleta> listadoCompletoCeldas, RecorridosDTO recorrido)
        {
            List<List<CeldaCompleta>> listaCeldasConIndice = new List<List<CeldaCompleta>>();
            List<CeldaCompleta> listaParcial = new List<CeldaCompleta>();
            bool enProgreso = false;
            int ultimoDia = 0;
            int ultimaCelda = 0;
            int posicionNueva;
            int posicionActual;

            foreach (var celda in listadoCompletoCeldas)
            {
                if (!enProgreso)
                {
                    celda.PosicionesPorRecorrido.TryGetValue(recorrido.Id, out posicionActual);
                    if (posicionActual == 0)
                    {
                        listaParcial = new List<CeldaCompleta>() { celda };
                        enProgreso = true;
                        ultimaCelda = 0;
                        ultimoDia = ObtenerDiaPorTimestamp.ObtenerDia(celda.DatasetHistorico.Tiempo_Request);
                    }
                }
                else
                {
                    var diaActualCelda = ObtenerDiaPorTimestamp.ObtenerDia(celda.DatasetHistorico.Tiempo_Request);
                    celda.PosicionesPorRecorrido.TryGetValue(recorrido.Id, out posicionNueva);

                    if (diaActualCelda == ultimoDia && ultimaCelda <= posicionNueva)
                    {
                        ultimaCelda = posicionNueva;
                        listaParcial.Add(celda);
                    }
                    else
                    {
                        enProgreso = false;
                        listaCeldasConIndice.Add(listaParcial);
                    }
                }

            }

            return listaCeldasConIndice;
        }

        private DiccionarioComplejo CrearDiccionarioComplejo(List<DatasetHistorico> datasetHistoricos, RecorridosDTO recorrido)
        {
            var listadoCompletoCeldas = GenerarCeldasConTodaInformacion(datasetHistoricos, recorrido);

            List<List<CeldaCompleta>> listaCeldasConIndice = GenerarListaConCeldasConIndice(listadoCompletoCeldas, recorrido);

            List<TiempoPorCantidadCeldas> tiempoPorCantidadCeldas = new List<TiempoPorCantidadCeldas>();
            double seconds = 0;
            int diferenciaCeldas = 0;
            int posicion1, posicion2;

            #region Generar objeto complejo de tipo diccionario

            DiccionarioComplejo diccionarioComplejo = new DiccionarioComplejo
            {
                DiccionarioClaves = new List<EntradaDiccionario>()
            };

            foreach (var listaCeldas in listaCeldasConIndice)
            {
                for (int i = 0; i < listaCeldas.Count - 1; i++)
                {
                    var primerValor = listaCeldas[i];
                    var segundoValor = listaCeldas[i + 1];
                    seconds = ObtenerDiferenciaDeTiempo(primerValor.DatasetHistorico.Tiempo_Request, segundoValor.DatasetHistorico.Tiempo_Request);

                    primerValor.PosicionesPorRecorrido.TryGetValue(recorrido.Id, out posicion1);
                    segundoValor.PosicionesPorRecorrido.TryGetValue(recorrido.Id, out posicion2);

                    diferenciaCeldas = posicion2 - posicion1;

                    if (diferenciaCeldas > 0 && diferenciaCeldas < 6 && seconds > 0 && seconds < 720)
                    {
                        var existeEntrada = diccionarioComplejo.DiccionarioClaves
                            .Where(x => x.PuntoOrigen == posicion1 && x.PuntoDestino == posicion2).ToList();

                        if (existeEntrada.Any())
                        {
                            var entradaExistente = existeEntrada.First();
                            entradaExistente.Segundos.Add(seconds);
                        }
                        else
                        {
                            diccionarioComplejo.DiccionarioClaves.Add(new EntradaDiccionario()
                            {
                                PuntoOrigen = posicion1,
                                PuntoDestino = posicion2,
                                Segundos = new List<double>() { seconds },
                            });
                        }
                    }

                }
            }

            diccionarioComplejo.DiccionarioClaves = diccionarioComplejo.DiccionarioClaves.OrderBy(x => x.PuntoDestino - x.PuntoOrigen).ToList();
            return diccionarioComplejo;

            #endregion
        }

        private double ObtenerDiferenciaDeTiempo(long timestamp1, long timestamp2)
        {
            try
            {
                var date1 = ObtenerFechaAPartirLong.ObtenerDateTime(timestamp1);
                var date2 = ObtenerFechaAPartirLong.ObtenerDateTime(timestamp2);
                return (date2 - date1).TotalSeconds;
            }
            catch (Exception ex)
            {
                Exception error = new Exception("Los tiempos ingresados no son validos", ex.InnerException);
                throw error;
            }
        }

        private int ObtenerFilaAsociada(List<double> filas, double valor)
        {
            try
            {
                for (int i = 0; i <= filas.Count; i++)
                {
                    if (valor > filas[i])
                    {
                        return i;
                    }
                }
                return filas.Count;
            }
            catch (Exception ex)
            {
                Exception error = new Exception("No se encontro una fila asociada", ex.InnerException);
                throw error;
            }
        }

        private int ObtenerColumnaAsociada(List<double> columnas, double valor)
        {
            try
            {
                for (int i = 0; i <= columnas.Count; i++)
                {
                    if (valor < columnas[i])
                    {
                        return i;
                    }
                }
                return columnas.Count;
            }
            catch (Exception ex)
            {
                Exception error = new Exception("No se encontro una columna asociada", ex.InnerException);
                throw error;
            }
        }

        public bool ContieneClaveValor(Dictionary<int, int> diccionario,
                             int keyEsperado, int valorEsperado)
        {
            int actualValue;
            if (!diccionario.TryGetValue(keyEsperado, out actualValue))
            {
                return false;
            }
            return actualValue == valorEsperado;
        }

        #endregion

    }
}