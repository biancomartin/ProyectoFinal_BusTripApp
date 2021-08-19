using Domain.Entities;
using Helper.Functions;
using Microsoft.Extensions.Logging;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.Services
{
    public class AnalizadorRecorridosService : IAnalizadorRecorridosService
    {

        #region Fields

        private readonly IRecorridoBaseService _recorridoBaseService;
        private readonly ILogger<AnalizadorRecorridosService> _logger;

        #endregion

        #region Constructor

        public AnalizadorRecorridosService(IRecorridoBaseService recorridoBaseService, ILogger<AnalizadorRecorridosService> logger)
        {
            _recorridoBaseService = recorridoBaseService;
            _logger = logger;
        }

        #endregion

        #region Public Methods

        public List<RecorridoMasCercano> ObtenerParadasCercanas(Coordenada coordenadaOrigen, Coordenada coordenadaDestino, List<int> lineas)
        {
            List<RecorridoMasCercano> response = new List<RecorridoMasCercano>();
            foreach(var linea in lineas) 
            {
                try
                {
                    List<RecorridoMasCercano> caminosAlternativos = new List<RecorridoMasCercano>();
                    var haversine = new Haversine();
                    var recorridos = _recorridoBaseService.ObtenerRecorridoBasePorLinea(linea);
                    foreach (var recorridoBase in recorridos)
                    {
                        double distanciaMinimaOrigen = double.MaxValue;
                        double distanciaMinimaDestino = double.MaxValue;
                        double distanciaParcialOrigen;
                        double distanciaParcialDestino;
                        int indiceOrigen = 0, indiceDestino = 0, index = 0;
                        var coordenadaMasCercanaOrigen = new Coordenada();
                        var coordenadaMasCercanaDestino = new Coordenada();
                        foreach (var punto in recorridoBase.Coordenadas)
                        {
                            distanciaParcialOrigen = haversine.Distance(punto, coordenadaOrigen, DistanceType.Kilometers);
                            distanciaParcialDestino = haversine.Distance(punto, coordenadaDestino, DistanceType.Kilometers);
                            if (distanciaParcialOrigen < distanciaMinimaOrigen)
                            {
                                distanciaMinimaOrigen = distanciaParcialOrigen;
                                coordenadaMasCercanaOrigen = punto;
                                indiceOrigen = index;
                            }
                            if (distanciaParcialDestino < distanciaMinimaDestino)
                            {
                                distanciaMinimaDestino = distanciaParcialDestino;
                                coordenadaMasCercanaDestino = punto;
                                indiceDestino = index;
                            }
                            index++;
                        }
                        if (indiceOrigen < indiceDestino)
                        {
                            caminosAlternativos.Add(new RecorridoMasCercano
                            {
                                CoordenadaOrigen = new Coordenada
                                {
                                    Latitude = coordenadaMasCercanaOrigen.Latitude,
                                    Longitude = coordenadaMasCercanaOrigen.Longitude
                                },
                                CoordenadaDestino = new Coordenada
                                {
                                    Latitude = coordenadaMasCercanaDestino.Latitude,
                                    Longitude = coordenadaMasCercanaDestino.Longitude
                                },
                                Trayecto = recorridoBase.Trayecto,
                                Linea = linea
                            });
                        };
                    }

                    var camino = DevolverRecorridoMasCercano(caminosAlternativos, coordenadaOrigen, coordenadaDestino);
                    ObtenerParadasIntermedias(camino, recorridos.FirstOrDefault(x => x.Trayecto == camino.Trayecto).Coordenadas);
                    response.Add(camino);
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex.Message, ex);
                }
            }

            return response;
        }

        #endregion

        #region Private Methods

        private RecorridoMasCercano DevolverRecorridoMasCercano(List<RecorridoMasCercano> caminosAlternativos, Coordenada coordenadaOrigen, Coordenada coordenadaDestino)
        {
            var haversine = new Haversine();
            RecorridoMasCercano response = new RecorridoMasCercano();
            if (!caminosAlternativos.Any())
            {
                throw new Exception("No se pudo encontrar un camino asociado a esa ubicacion");
            }
            if (caminosAlternativos.Count == 1)
            {
                return caminosAlternativos.FirstOrDefault();
            }
            if (caminosAlternativos.Count > 1)
            {
                var distanciaMinima = double.MaxValue;
                foreach (var camino in caminosAlternativos)
                {
                    var distanciaOrigenParcial = haversine.Distance(camino.CoordenadaOrigen, coordenadaOrigen, DistanceType.Kilometers);
                    var distanciaDestinoParcial = haversine.Distance(camino.CoordenadaDestino, coordenadaDestino, DistanceType.Kilometers);
                    double distanciaMinimaParcial = (distanciaOrigenParcial + distanciaDestinoParcial);
                    if (distanciaMinimaParcial < distanciaMinima)
                    {
                        distanciaMinima = distanciaMinimaParcial;
                        response = camino;
                    }
                }
                return response;
            }
            return response;
        }

        private void ObtenerParadasIntermedias(RecorridoMasCercano recorridoMasCercano, IEnumerable<Coordenada> recorridoBase)
        {
            try
            {
                recorridoMasCercano.CoordenadasIntermedias = new List<Coordenada>();
                var coordenadas = new List<Coordenada>();

                var indexOrigen = recorridoBase.ToList().FindIndex(x => x.Latitude == recorridoMasCercano.CoordenadaOrigen.Latitude
                                    && x.Longitude == recorridoMasCercano.CoordenadaOrigen.Longitude);
                var indexDestino = recorridoBase.ToList().FindIndex(x => x.Latitude == recorridoMasCercano.CoordenadaDestino.Latitude
                        && x.Longitude == recorridoMasCercano.CoordenadaDestino.Longitude);
                recorridoMasCercano.CoordenadasIntermedias =
                    recorridoBase.ToList().GetRange(indexOrigen, indexDestino - indexOrigen + 1);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al Obtener las paradas intermedias", ex.InnerException);
            }
        }

        #endregion

    }
}
