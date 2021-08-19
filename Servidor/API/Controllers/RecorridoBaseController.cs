using ColectivosApi.InputEntities;
using ColectivosApi.Mappers;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;

namespace ColectivosApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class RecorridoBaseController : ControllerBase
    {
        #region Fields

        private readonly IAnalizadorRecorridosService _analizadorRecorridosService;
        private readonly IRecorridoBaseService _recorridoBaseService;

        #endregion

        #region Constructor

        public RecorridoBaseController(IAnalizadorRecorridosService analizadorRecorridosService, IRecorridoBaseService recorridoBaseService)
        {
            _analizadorRecorridosService = analizadorRecorridosService;
            _recorridoBaseService = recorridoBaseService;
        }

        #endregion

        #region Endpoints

        /// <summary>
        /// A partir de una coordenada y un recorrido, se retorna la parada mas cercana
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///         "coordenada": {
        ///             "latitude": -37.330665,
        ///             "longitude": -59.147929
        ///         },
        ///         "linea": 500,
        ///         "trayecto": 1
        ///     }
        ///
        /// </remarks>
        [HttpPost("ParadaMasCercanaPorLinea")]
        public ActionResult ObtenerParadaMasCercanaPorLinea([FromBody] CoordenadaConRecorridoInput input)
        {
            try
            {
                var coordenada = _recorridoBaseService.ObtenerParadaMasCercana(input.Coordenada, input.Linea, input.Trayecto);
                return Ok(coordenada);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        /// <summary>
        /// Obtiene los recorridos a partir de una linea dada
        /// </summary>
        /// <param name="linea"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     Get /500
        ///
        /// </remarks>
        [HttpGet("ObtenerRecorridosPorLinea")]
        public ActionResult ObtenerRecorridoBase(int linea)
        {
            try
            {
                var recorridosBase = _recorridoBaseService.ObtenerRecorridoBasePorLinea(linea);
                return Ok(recorridosBase);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Retonar todos los recorridos disponibles
        /// </summary>
        /// <returns></returns>
        [HttpGet("ObtenerRecorridos")]
        public ActionResult ObtenerRecorridos()
        {
            try
            {
                var recorridos = _recorridoBaseService.ObtenerRecorridos();
                return Ok(recorridos);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        [HttpGet("EjemplosCoordenadas")]
        public ActionResult ObtenerEjemplosCoordenadas()
        {
            try
            {
                var coordenadas = new List<Coordenada>
                {
                    new Coordenada { Latitude = -37.330585, Longitude = -59.132435},
                    new Coordenada { Latitude = -37.32939, Longitude = -59.133106},
                    new Coordenada { Latitude = -37.32936, Longitude = -59.13312},
                    new Coordenada { Latitude = -37.32902, Longitude = -59.13333},
                    new Coordenada { Latitude = -37.32838, Longitude = -59.133858},
                    new Coordenada { Latitude = -37.328865, Longitude = -59.135166},
                    new Coordenada { Latitude = -37.32897, Longitude = -59.135456},
                    new Coordenada { Latitude = -37.327515, Longitude = -59.137646},
                    new Coordenada { Latitude = -37.32622, Longitude = -59.138424},
                    new Coordenada { Latitude = -37.325184, Longitude = -59.1376},
                    new Coordenada { Latitude = -37.324837, Longitude = -59.136715},
                    new Coordenada { Latitude = -37.32461, Longitude = -59.136017},
                    new Coordenada { Latitude = -37.324364, Longitude = -59.135437},
                    new Coordenada { Latitude = -37.323795, Longitude = -59.133957},
                    new Coordenada { Latitude = -37.323498, Longitude = -59.13307},
                    new Coordenada { Latitude = -37.32302, Longitude = -59.131855},
                    new Coordenada { Latitude = -37.322906, Longitude = -59.13157},
                    new Coordenada { Latitude = -37.322655, Longitude = -59.131554},
                    new Coordenada { Latitude = -37.321228, Longitude = -59.1324},
                    new Coordenada { Latitude = -37.32074, Longitude = -59.13268},
                    new Coordenada { Latitude = -37.31982, Longitude = -59.13323},
                    new Coordenada { Latitude = -37.318325, Longitude = -59.13413},
                    new Coordenada { Latitude = -37.317417, Longitude = -59.134686},
                    new Coordenada { Latitude = -37.3168, Longitude = -59.135036},
                    new Coordenada { Latitude = -37.315914, Longitude = -59.135563},
                    new Coordenada { Latitude = -37.314774, Longitude = -59.13624},
                    new Coordenada { Latitude = -37.31454, Longitude = -59.136368},
                    new Coordenada { Latitude = -37.31386, Longitude = -59.13677},
                    new Coordenada { Latitude = -37.313152, Longitude = -59.1372},
                    new Coordenada { Latitude = -37.31247, Longitude = -59.137585},
                    new Coordenada { Latitude = -37.311783, Longitude = -59.13797},
                    new Coordenada { Latitude = -37.311207, Longitude = -59.138317},
                    new Coordenada { Latitude = -37.310863, Longitude = -59.138172},
                    new Coordenada { Latitude = -37.3102, Longitude = -59.136433},
                    new Coordenada { Latitude = -37.309517, Longitude = -59.1354},
                    new Coordenada { Latitude = -37.308487, Longitude = -59.13674},
                    new Coordenada { Latitude = -37.30743, Longitude = -59.13812},
                    new Coordenada { Latitude = -37.30656, Longitude = -59.139233},
                    new Coordenada { Latitude = -37.306293, Longitude = -59.13958},
                    new Coordenada { Latitude = -37.305336, Longitude = -59.14082},
                    new Coordenada { Latitude = -37.30467, Longitude = -59.141678},
                    new Coordenada { Latitude = -37.30355, Longitude = -59.14272},
                    new Coordenada { Latitude = -37.302853, Longitude = -59.141834},
                    new Coordenada { Latitude = -37.30185, Longitude = -59.140614},
                    new Coordenada { Latitude = -37.30131, Longitude = -59.139942},
                    new Coordenada { Latitude = -37.300705, Longitude = -59.139187},
                    new Coordenada { Latitude = -37.29973, Longitude = -59.137966},
                    new Coordenada { Latitude = -37.29904, Longitude = -59.137115},
                    new Coordenada { Latitude = -37.297974, Longitude = -59.13627},
                    new Coordenada { Latitude = -37.297283, Longitude = -59.13713}
                };
                return Ok(coordenadas);
            }
            catch (Exception ex)
            {
                return Ok($"Error, {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene las coordenadas posibles de cada recorrido
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///         "coordenadaOrigen": {
        ///             "latitude": -37.332015, 
        ///             "longitude": -59.143546
        ///         },
        ///         "coordenadaDestino": {
        ///             "latitude": -37.317505, 
        ///             "longitude": -59.132795
        ///         },
        ///         "lineas": [500, 501, 502, 503, 504, 505]
        ///     }
        ///
        /// </remarks>
        [HttpPost("ParadasCercanas")]
        public ActionResult ObtenerParadasCercanas([FromBody] CaminosAlternativosInput input)
        {
            try
            {
                var caminosPosibles = _analizadorRecorridosService.ObtenerParadasCercanas(input.CoordenadaOrigen, input.CoordenadaDestino, input.Lineas);
                if (caminosPosibles == null)
                {
                    return StatusCode((int)HttpStatusCode.NoContent, "Ningun recorrido conveniente para esas ubicaciones");
                }
                return Ok(ParadaMasCercanasMapper.ObtenerParadasMasCercanas(caminosPosibles));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        #endregion
    }
}