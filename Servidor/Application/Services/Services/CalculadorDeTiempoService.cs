using Domain.Dataset;
using Domain.Diccionario;
using Domain.Entities;
using Domain.Matriz;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Regresion.Interfaces;
using Services.Interfaces;
using System;
using System.Linq;

namespace Services.Services
{
    public class CalculadorDeTiempoService : ICalculadorDeTiempoService
    {
        #region Fields

        private readonly IColectivosService _colectivosService;
        private readonly IDiccionarioService _diccionarioService;
        private readonly IFranjaHorariaRepository _franjaHorariaRepository;
        private readonly IRegresionDiferenciaCeldasService _regresionDiferenciaCeldasService;
        private readonly ILogger<CalculadorDeTiempoService> _logger;

        #endregion

        #region Constructor

        public CalculadorDeTiempoService(IColectivosService colectivosService, 
                                            IDiccionarioService diccionarioService, 
                                            IFranjaHorariaRepository franjaHorariaRepository,
                                            IRegresionDiferenciaCeldasService regresionDiferenciaCeldasService,
                                            ILogger<CalculadorDeTiempoService> logger)
        {
            _colectivosService = colectivosService;
            _diccionarioService = diccionarioService;
            _franjaHorariaRepository = franjaHorariaRepository;
            _regresionDiferenciaCeldasService = regresionDiferenciaCeldasService;
            _logger = logger;
        }

        #endregion

        #region Public Methods

        public double ObtenerTiempoEntreCoordenadasComplejo(Coordenada posicionOrigen, Coordenada posicionDestino, DateTime date, int trayecto, int linea, int? unidadId)
        {
            try
            {
                CeldaMatriz celdaOrigen = ObtenerCeldaSegunCoordenada(posicionOrigen);
                CeldaMatriz celdaDestino = ObtenerCeldaSegunCoordenada(posicionDestino);

                int indiceRecorridoOrigen = _colectivosService.ObtenerIndiceDeRecorridoBase(celdaOrigen.Fila, celdaOrigen.Columna, trayecto, linea);
                int indiceRecorridoDestino = _colectivosService.ObtenerIndiceDeRecorridoBase(celdaDestino.Fila, celdaDestino.Columna, trayecto, linea);

                if (indiceRecorridoDestino < indiceRecorridoOrigen)
                {
                    throw new Exception("Posiciones Invalidas");
                }

                var franjaHoraria = _franjaHorariaRepository.ObtenerFranjaPorFecha(date);
                var diccionarios = _diccionarioService.ObtenerDiccionarioComplejoPorFranja(franjaHoraria.Id, trayecto, linea, unidadId);

                if (!diccionarios.DiccionarioClaves.Any())
                {
                    throw new Exception("Sin informacion disponible");
                }

                var parTiempoCantidad = ObtenerTiempoConDiccionarioComplejo(indiceRecorridoOrigen, indiceRecorridoDestino, diccionarios, trayecto, franjaHoraria.Id, linea);
                return parTiempoCantidad.TiempoAcumulado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al calcular el tiempo", ex.InnerException);
            }
        }

        public RegresionMetricas ObtenerTiempoPorRegresionDiferenciaDeCeldas(Coordenada posicionOrigen, Coordenada posicionDestino, DateTime fecha, int trayecto, int linea, int? unidadId)
        {
            try
            {
                CeldaMatriz celdaOrigen = ObtenerCeldaSegunCoordenada(posicionOrigen);
                CeldaMatriz celdaDestino = ObtenerCeldaSegunCoordenada(posicionDestino);

                int indiceRecorridoOrigen = _colectivosService.ObtenerIndiceDeRecorridoBase(celdaOrigen.Fila, celdaOrigen.Columna, trayecto, linea);
                int indiceRecorridoDestino = _colectivosService.ObtenerIndiceDeRecorridoBase(celdaDestino.Fila, celdaDestino.Columna, trayecto, linea);

                if (indiceRecorridoDestino < indiceRecorridoOrigen)
                {
                    throw new Exception("Posiciones Invalidas");
                }

                var diferenciaDeCeldas = indiceRecorridoDestino - indiceRecorridoOrigen;
                var unidad = unidadId.HasValue ? unidadId.Value.ToString() : null;
                var franjaHoraria = _franjaHorariaRepository.ObtenerFranjaPorFecha(fecha);

                var response = _regresionDiferenciaCeldasService.PredecirValor(new Regresion.Entities.DatasetModel()
                {
                    DiferenciaCeldas = diferenciaDeCeldas,
                    FranjaHorariaId = franjaHoraria.Id.ToString(),
                    LineaId = linea.ToString(),
                    RecorridoId = trayecto.ToString(),
                    UnidadId = unidad
                });

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al calcular el tiempo", ex.InnerException);
            }
        }
    
        #endregion

        #region Private Methods

        private CeldaMatriz ObtenerCeldaSegunCoordenada(Coordenada pos)
        {
            try
            {
                CeldaMatriz response = new CeldaMatriz()
                {
                    Fila = _colectivosService.ObtenerIndiceFilaLatitud(pos.Latitude),
                    Columna = _colectivosService.ObtenerIndiceColumnaLongitud(pos.Longitude)
                };
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                Exception error = new Exception("Coordenada no perteneciente a la matriz", ex.InnerException);
                throw error;
            }
        }

        private ParTiempoCantidadDeMuestras ObtenerTiempoConDiccionarioComplejo(int indiceRecorridoOrigen, int indiceRecorridoDestino, DiccionarioComplejo diccionario, int trayecto, int franjaHorariaId, int linea)
        {
            try
            {
                if (indiceRecorridoDestino - indiceRecorridoOrigen == 0)
                {
                    return new ParTiempoCantidadDeMuestras()
                    {
                        TiempoAcumulado = 0,
                        CantidadMuestras = 0
                    };
                }
                if (indiceRecorridoDestino - indiceRecorridoOrigen == 1)
                {
                    var clavesParciales = diccionario.DiccionarioClaves
                        .Where(x => x.PuntoOrigen == indiceRecorridoOrigen && x.PuntoDestino == indiceRecorridoDestino).FirstOrDefault();
                    if (clavesParciales != null)
                    {
                        return new ParTiempoCantidadDeMuestras()
                        {
                            TiempoAcumulado = clavesParciales.Segundos.Sum() / clavesParciales.Segundos.Count,
                            CantidadMuestras = clavesParciales.CantidadDeMuestras
                        };
                    }
                    else
                    {
                        return TiempoEntreDosPosiciones(indiceRecorridoOrigen, indiceRecorridoDestino, trayecto, franjaHorariaId, linea);
                    }
                }

                int diferencia = (indiceRecorridoDestino - indiceRecorridoOrigen) / 2;
                var primerMitadDeTrayecto = ObtenerTiempoConDiccionarioComplejo(indiceRecorridoOrigen, indiceRecorridoOrigen + diferencia, diccionario, trayecto, franjaHorariaId, linea);
                ParTiempoCantidadDeMuestras segundaMitadTrayecto = new ParTiempoCantidadDeMuestras() { TiempoAcumulado = 0, CantidadMuestras = 0 };
                segundaMitadTrayecto = ObtenerTiempoConDiccionarioComplejo(indiceRecorridoOrigen + diferencia, indiceRecorridoDestino, diccionario, trayecto, franjaHorariaId, linea);

                var response = TiempoPromedioEntreMitades(indiceRecorridoOrigen, indiceRecorridoDestino, diccionario, primerMitadDeTrayecto, segundaMitadTrayecto);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al calcular el tiempo con el diccionario provisto", ex.InnerException);
            }

        }

        private ParTiempoCantidadDeMuestras TiempoPromedioEntreMitades(int indiceOrigen, int indiceDestino, DiccionarioComplejo diccionario,
            ParTiempoCantidadDeMuestras parteInicial, ParTiempoCantidadDeMuestras parteFinal)
        {
            ParTiempoCantidadDeMuestras response = new ParTiempoCantidadDeMuestras()
            {
                TiempoAcumulado = parteInicial.TiempoAcumulado + parteFinal.TiempoAcumulado,
                CantidadMuestras = parteInicial.CantidadMuestras + parteFinal.CantidadMuestras
            };
            var celdasCompleta = diccionario.DiccionarioClaves.FirstOrDefault(x => x.PuntoOrigen == indiceOrigen && x.PuntoDestino == indiceDestino);

            if (celdasCompleta != null)
            {
                double cantidadMuestrasDeLasPartes = (parteInicial.CantidadMuestras + parteFinal.CantidadMuestras) / 2;
                var totalMuestras = celdasCompleta.CantidadDeMuestras + cantidadMuestrasDeLasPartes;
                var tiempoCompleto = celdasCompleta.Segundos.Sum() / celdasCompleta.Segundos.Count;
                var tiempo = (response.TiempoAcumulado * ((double)cantidadMuestrasDeLasPartes / (double)totalMuestras))
                                    + (tiempoCompleto * ((double)celdasCompleta.CantidadDeMuestras / (double)totalMuestras));

                return new ParTiempoCantidadDeMuestras()
                {
                    TiempoAcumulado = tiempo,
                    CantidadMuestras = (int)totalMuestras
                };
            }

            return response;
        }

        private ParTiempoCantidadDeMuestras TiempoEntreDosPosiciones(int indiceOrigen, int indiceFinal, int trayecto, int franjaHorariaId, int linea)
        {
            var diccionariosPorFranja = _diccionarioService.ObtenerDiccionarioComplejoPorFranja(franjaHorariaId, trayecto, linea, null);
            var clavesParciales = diccionariosPorFranja.DiccionarioClaves
                        .Where(x => x.PuntoOrigen == indiceOrigen && x.PuntoDestino == indiceFinal).FirstOrDefault();
            if (clavesParciales != null && clavesParciales.CantidadDeMuestras > 20)
            {
                var response = ObtenerTiempoConDiccionarioComplejo(indiceOrigen, indiceFinal, diccionariosPorFranja, trayecto, franjaHorariaId, linea);
                return response;
            }
            else
            {
                var response = ObtenerTiempoConDiccionarioComplejo(indiceOrigen-1, indiceFinal-1, diccionariosPorFranja, trayecto, franjaHorariaId, linea);
                return response;
            }
        }

        #endregion
    }
}