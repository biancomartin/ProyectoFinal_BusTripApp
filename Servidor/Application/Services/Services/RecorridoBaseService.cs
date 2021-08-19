using Domain.DTOs;
using Domain.Entities;
using Domain.RecorridoBase;
using Domain.Interfaces;
using Helper.Functions;
using Helper.RecorridoBase;
using Microsoft.Extensions.Logging;
using Services.Interfaces;
using SQLRepository.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.Services
{
    public class RecorridoBaseService : IRecorridoBaseService
    {
        #region Fields

        private readonly IRecorridosRepository _recorridosRepository;
        private readonly IParadasRepository _paradasRepository;
        private readonly ILogger<RecorridoBaseService> _logger;

        #endregion

        #region Constructor

        public RecorridoBaseService(IRecorridosRepository recorridosRepository, 
                                    IParadasRepository paradasRepository,
                                    ILogger<RecorridoBaseService> logger)
        {
            _recorridosRepository = recorridosRepository;
            _paradasRepository = paradasRepository;
            _logger = logger;
        }

        #endregion

        #region Public Methods

        public List<Coordenada> ObtenerRecorridoBasePorLineaYTrayecto(int linea, int trayecto)
        {
            var recorrido = _recorridosRepository.ObtenerRecorridoPorLineaYTrayecto(linea, trayecto);
            var paradasPorRecorrido = _paradasRepository.ObtenerParadasPorRecorridoId(recorrido.Id);
            if (paradasPorRecorrido != null)
            {
                return MapperCoordenada.ObtenerCoordenadas(paradasPorRecorrido);
            }
            else
            {
                throw new Exception("Numero de linea invalido");
            }
        }

        public List<RecorridoBasePorRecorrido> ObtenerRecorridoBasePorLinea(int linea)
        {
            var response = new List<RecorridoBasePorRecorrido>();
            var recorridos = _recorridosRepository.ObtenerRecorridos().Where(x => x.Linea == linea);
            if (!recorridos.Any())
            {
                throw new Exception("Numero de linea invalido");
            }
            foreach(var recorrido in recorridos)
            {
                var coordenadas = new List<Coordenada>();
                var coordenadasDTO = _paradasRepository.ObtenerParadasPorRecorridoId(recorrido.Id);
                coordenadas = MapperCoordenada.ObtenerCoordenadas(coordenadasDTO);
                response.Add(
                    new RecorridoBasePorRecorrido
                    {
                        Linea = linea,
                        Trayecto = recorrido.Base,
                        Coordenadas = coordenadas
                    });
            }
            return response;
        }

        public Coordenada ObtenerParadaMasCercana(Coordenada coordenada, int linea, int trayecto)
        {
            var recorridoBase = ObtenerRecorridoBasePorLineaYTrayecto(linea, trayecto);
            double minDistance = double.MaxValue;
            double distanciaParcial;
            var coordenadaMasCercana = new Coordenada();
            var haversine = new Haversine();
            foreach (var punto in recorridoBase)
            {
                distanciaParcial = haversine.Distance(punto, coordenada, DistanceType.Kilometers);
                if (minDistance > distanciaParcial)
                {
                    minDistance = distanciaParcial;
                    coordenadaMasCercana = punto;
                }
            }
            return coordenadaMasCercana;
        }

        public List<RecorridosDTO> ObtenerRecorridos()
        {
            try
            {
                var recorridos = _recorridosRepository.ObtenerRecorridos();
                foreach (var recorrido in RecorridosAConsiderar.recorridos)
                {
                    if (!recorridos.Any(x => x.Id == recorrido.Id))
                    {
                        this._recorridosRepository.InsertarRecorrido(recorrido);
                    }
                }
                var recorridosFinal = _recorridosRepository.ObtenerRecorridos();
                return recorridosFinal;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Fallo al generar/obtener los recorridos", ex.InnerException);
            }
        }

        #endregion

    }
}