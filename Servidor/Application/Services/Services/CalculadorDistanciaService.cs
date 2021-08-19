using Domain.Entities;
using Domain.Matriz;
using Domain.Interfaces;
using Helper.Functions;
using Microsoft.Extensions.Logging;
using Services.Interfaces;
using System;
using System.Linq;

namespace Services.Services
{
    public class CalculadorDistanciaService : ICalculadorDistanciaService
    {
        #region Fields

        private readonly IRecorridosRepository _recorridosRepository;
        private readonly IParadasRepository _paradasRepository;
        private readonly IColectivosService _colectivosService;
        private readonly ILogger<CalculadorDistanciaService> _logger;

        #endregion

        #region Constructor

        public CalculadorDistanciaService(IRecorridosRepository recorridosRepository, 
                                            IParadasRepository paradasRepository,
                                            IColectivosService colectivosService,
                                            ILogger<CalculadorDistanciaService> logger)
        {
            _recorridosRepository = recorridosRepository;
            _paradasRepository = paradasRepository;
            _colectivosService = colectivosService;
            _logger = logger;
        }

        #endregion

        #region Public Methods

        public double CalcularDistancia(Coordenada origen, Coordenada destino, int linea, int trayecto)
        {
            try
            {
                var recorrido = _recorridosRepository.ObtenerRecorridoPorLineaYTrayecto(linea, trayecto);
                var paradasBase = _paradasRepository.ObtenerParadasPorRecorridoId(recorrido.Id);
                CeldaMatriz celda1 = ObtenerCeldaSegunCoordenada(origen);
                CeldaMatriz celda2 = ObtenerCeldaSegunCoordenada(destino);
                int indiceOrigen = _colectivosService.ObtenerIndiceDeRecorridoBase(celda1.Fila, celda1.Columna, trayecto, linea);
                int indiceDestino = _colectivosService.ObtenerIndiceDeRecorridoBase(celda2.Fila, celda2.Columna, trayecto, linea);
                var paradasIntermedias = paradasBase.GetRange(indiceOrigen, indiceDestino - indiceOrigen + 1);
                if (indiceOrigen < 0 || indiceDestino < 0 || indiceDestino < indiceOrigen || !paradasIntermedias.Any())
                {
                    return 0;
                }
                double distancia = 0;
                Haversine haversine = new Haversine();
                for (int i = 0; i < paradasIntermedias.Count - 1; i++)
                {
                    var pos1 = paradasIntermedias[i];
                    var pos2 = paradasIntermedias[i + 1];
                    distancia += haversine.Distance(new Coordenada { Latitude = pos1.Latitud, Longitude = pos1.Longitud },
                        new Coordenada { Latitude = pos2.Latitud, Longitude = pos2.Longitud }, DistanceType.Kilometers);
                }
                return distancia;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al calcular la distancia", ex.InnerException);
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

        #endregion

    }
}
