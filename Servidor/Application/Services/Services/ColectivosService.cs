using Domain.DTOs;
using Domain.Entities.Dataset;
using Domain.Interfaces;
using Domain.Matriz;
using Helper.DimensionesMatriz;
using Helper.ExtremosMatriz;
using Helper.Functions;
using Microsoft.Extensions.Logging;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.Services
{
    public class ColectivosService : IColectivosService
    {

        #region Constantes

        public const int NumeroColumnas = DimensionesMatriz.NumeroColumnas;
        public const int NumeroFilas = DimensionesMatriz.NumeroFilas;

        #endregion

        #region Fields

        public List<CeldaMatriz> azulCeldas = new List<CeldaMatriz>();
        public List<CeldaMatriz> rojoCeldas = new List<CeldaMatriz>();

        private readonly List<double> FilasMatriz;
        private readonly List<double> ColumnasMatriz;
        private readonly IRecorridoBaseService _recorridoBaseService;
        private readonly IDatasetRepository _datasetRepository;
        private readonly ILogger<ColectivosService> _logger;

        #endregion

        #region Constructor

        public ColectivosService(IRecorridoBaseService recorridoBaseService,
                                    IDatasetRepository datasetRepository,
                                    ILogger<ColectivosService> logger)
        {
            _recorridoBaseService = recorridoBaseService;
            _datasetRepository = datasetRepository;
            _logger = logger;

            #region Columnas (long)

            var direrencia_columnas = (ExtremosMatriz.LongitudDerecha - ExtremosMatriz.LongitudIzquierda) / (NumeroColumnas - 1);
            double min_col = ExtremosMatriz.LongitudIzquierda;
            ColumnasMatriz = new List<double>();
            for (int i = 0; i <= NumeroColumnas; i++)
            {
                ColumnasMatriz.Add(min_col);
                min_col += direrencia_columnas;
            }

            ColumnasMatriz = ColumnasMatriz.OrderBy(x => x).ToList();

            #endregion

            #region Filas (lat)

            var direrencia_filas = ((ExtremosMatriz.LatitudSuperior) - (ExtremosMatriz.LatitudInferior)) / (NumeroFilas - 1);

            double max_row = ExtremosMatriz.LatitudSuperior;
            FilasMatriz = new List<double>();
            for (int i = 0; i <= NumeroFilas; i++)
            {
                FilasMatriz.Add(max_row);
                max_row -= direrencia_filas;
            }

            #endregion
        }

        #endregion

        #region Public Methods

        public List<double> ObtenerFilasMatriz() => FilasMatriz;

        public List<double> ObtenerColumnasMatriz() => ColumnasMatriz;

        public int ObtenerIndiceDeRecorridoBase(int fila, int columna, int trayecto, int linea)
        {
            try
            {
                var celdas = GenerarCeldasAPartirDeLineaYTrayecto(linea, trayecto);
                for (int i = 0; i < celdas.Count; i++)
                {
                    if (celdas[i].Columna == columna && celdas[i].Fila == fila)
                    {
                        return i;
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                Exception error = new Exception("Indice no encontrado, Ingrese fila y columna validos", ex.InnerException);
                throw error;
            }
        }

        public int ObtenerIndiceFilaLatitud(double value)
        {
            for (int i = 0; i <= FilasMatriz.Count; i++)
            {
                if (value > FilasMatriz[i])
                {
                    return i;
                }
            }
            return FilasMatriz.Count;
        }

        public int ObtenerIndiceColumnaLongitud(double value)
        {
            for (int i = 0; i <= ColumnasMatriz.Count; i++)
            {
                if (value < ColumnasMatriz[i])
                {
                    return i;
                }
            }
            return ColumnasMatriz.Count;
        }

        public List<CeldaMatriz> GenerarCeldasAPartirDeLineaYTrayecto(int linea, int trayecto)
        {
            var celdas = new List<CeldaMatriz>();
            var coordenadas = _recorridoBaseService.ObtenerRecorridoBasePorLineaYTrayecto(linea, trayecto);

            try
            {
                foreach (var coordenada in coordenadas)
                {
                    var filaParcial = ObtenerIndiceFilaLatitud(coordenada.Latitude);
                    var columnaParcial = ObtenerIndiceColumnaLongitud(coordenada.Longitude);
                    CeldaMatriz celdaParaAgregar = new CeldaMatriz()
                    {
                        Columna = columnaParcial,
                        Fila = filaParcial
                    };
                    if (!celdas.Any(x => x.Fila == celdaParaAgregar.Fila && x.Columna == celdaParaAgregar.Columna))
                    {
                        celdas.Add(celdaParaAgregar);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                Console.WriteLine($"Error al generar las celdas del recorrido azul. {ex.Message}");
            }
            
            return celdas;
        }

        public List<DatasetHistorico> ObtenerDatasetColectivosPorHorario(FranjaHorariaDTO franjaHoraria)
        {
            List<DatasetHistorico> datasetResponse = new List<DatasetHistorico>();
            var dataset = _datasetRepository.ObtenerColectivosHistoricos();
            bool diaDeSemana;
            long tiempoEntrada;
            foreach (var entrada in dataset)
            {
                tiempoEntrada = entrada.Tiempo_Request;
                var dtDateTime = ObtenerFechaAPartirLong.ObtenerDateTime(tiempoEntrada);
                if (dtDateTime.Hour <= franjaHoraria.HoraFin && dtDateTime.Hour >= franjaHoraria.HoraInicio)
                {
                    diaDeSemana = ((int)dtDateTime.DayOfWeek == 0 || (int)dtDateTime.DayOfWeek == 6) ? true : false;
                    if (diaDeSemana == franjaHoraria.FinDeSemana)
                    {
                        datasetResponse.Add(entrada);
                    }
                }
            }

            return datasetResponse;
        }

        #endregion

    }
}