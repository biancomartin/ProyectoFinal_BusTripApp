using ColectivosApi.InputEntities;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;

namespace ColectivosApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalculadorTiempoController : ControllerBase
    {
        #region Fields

        private readonly ICalculadorDeTiempoService _calculadorDeTiempoService;
        private readonly ICalculadorDistanciaService _calculadorDistanciaService;

        #endregion

        #region Constructor
        
        public CalculadorTiempoController(ICalculadorDeTiempoService calculadorDeTiempoService, ICalculadorDistanciaService calculadorDistanciaService)
        {
            _calculadorDeTiempoService = calculadorDeTiempoService;
            _calculadorDistanciaService = calculadorDistanciaService;
        }

        #endregion

        #region Endpoints

        /// <summary>
        /// Calcula el tiempo a traves de una regresion segun la diferencia de celdas
        /// </summary>
        /// <param name="tiemposRequest"></param>
        /// <response code="200">Tiempo correspondiente</response>
        /// <response code="500">Error interno</response>
        /// <remarks>
        /// Sample request:
        ///
        ///     [{
        ///         "posicionOrigen": {
        ///             "latitude": -37.292771,
        ///             "longitude": -59.152198
        ///         },
        ///         "posicionDestino": {
        ///             "latitude": -37.291008,
        ///             "longitude": -59.156845
        ///         },
        ///         "fecha": "2020-08-17T17:00:58.291Z",
        ///         "trayecto": 1,
        ///         "lineaId": 500,
        ///         "unidadId": 1
        ///     },
        ///     {
        ///         "posicionOrigen": {
        ///             "latitude": -37.290173,
        ///             "longitude": -59.160275
        ///         },
        ///         "posicionDestino": {
        ///             "latitude": -37.309150,
        ///             "longitude": -59.138182
        ///         },
        ///         "fecha": "2020-08-17T10:00:58.291Z",
        ///         "trayecto": 2,
        ///         "lineaId": 500
        ///     }]
        ///
        /// </remarks>
        [HttpPost("RegresionLinealMultiple")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult RegresionLinealMultiple([FromBody] IEnumerable<CalcularTiempoInput> tiemposRequest)
        {
            try
            {
                var response = new List<ParTiempoDistancia>();
                foreach (var tiempoRequest in tiemposRequest)
                {
                    var tiempo = _calculadorDeTiempoService.ObtenerTiempoPorRegresionDiferenciaDeCeldas(
                                       tiempoRequest.PosicionOrigen, tiempoRequest.PosicionDestino, tiempoRequest.Fecha, tiempoRequest.Trayecto, tiempoRequest.LineaId, tiempoRequest.UnidadId);
                    var distancia = _calculadorDistanciaService.CalcularDistancia(tiempoRequest.PosicionOrigen, tiempoRequest.PosicionDestino, tiempoRequest.LineaId, tiempoRequest.Trayecto);
                    response.Add(new ParTiempoDistancia
                    {
                        Distancia = distancia,
                        Tiempo = tiempo.Predicction,
                        CoordenadaOrigen = tiempoRequest.PosicionOrigen,
                        CoordenadaDestino = tiempoRequest.PosicionDestino,
                        Linea = tiempoRequest.LineaId,
                        Trayecto = tiempoRequest.Trayecto
                    });
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Calcula el tiempo utilizando el enfoque matriz
        /// </summary>
        /// <param name="tiemposRequest"></param>
        /// <response code="200">Tiempo correspondiente</response>
        /// <response code="500">Error interno</response>    
        /// <remarks>
        /// Sample request:
        ///
        ///     [{
        ///         "posicionOrigen": {
        ///             "latitude": -37.292771,
        ///             "longitude": -59.152198
        ///         },
        ///         "posicionDestino": {
        ///             "latitude": -37.291008,
        ///             "longitude": -59.156845
        ///         },
        ///         "fecha": "2020-08-17T17:00:58.291Z",
        ///         "trayecto": 1,
        ///         "lineaId": 500,
        ///         "unidadId": 1
        ///     },
        ///     {
        ///         "posicionOrigen": {
        ///             "latitude": -37.290173,
        ///             "longitude": -59.160275
        ///         },
        ///         "posicionDestino": {
        ///             "latitude": -37.309150,
        ///             "longitude": -59.138182
        ///         },
        ///         "fecha": "2020-08-17T10:00:58.291Z",
        ///         "trayecto": 2,
        ///         "lineaId": 500
        ///     }]
        ///
        /// </remarks>
        [HttpPost("EnfoqueMatricial")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult EnfoqueMatricial([FromBody] IEnumerable<CalcularTiempoInput> tiemposRequest)
        {
            try
            {
                var response = new List<ParTiempoDistancia>();
                foreach (var tiempoRequest in tiemposRequest)
                {
                    var tiempo = _calculadorDeTiempoService.ObtenerTiempoEntreCoordenadasComplejo(
                        tiempoRequest.PosicionOrigen, tiempoRequest.PosicionDestino, tiempoRequest.Fecha, tiempoRequest.Trayecto, tiempoRequest.LineaId, tiempoRequest.UnidadId);
                    var distancia = _calculadorDistanciaService.CalcularDistancia(tiempoRequest.PosicionOrigen, tiempoRequest.PosicionDestino, tiempoRequest.LineaId, tiempoRequest.Trayecto);
                    response.Add(new ParTiempoDistancia
                    {
                        Distancia = distancia,
                        Tiempo = tiempo,
                        CoordenadaOrigen = tiempoRequest.PosicionOrigen,
                        CoordenadaDestino = tiempoRequest.PosicionDestino,
                        Linea = tiempoRequest.LineaId,
                        Trayecto = tiempoRequest.Trayecto
                    });
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        #endregion

    }
}
