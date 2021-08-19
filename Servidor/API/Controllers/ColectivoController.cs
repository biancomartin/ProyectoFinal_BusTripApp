using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System;
using System.Net;

namespace ColectivosApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ColectivoController : ControllerBase
    {

        #region Fields

        private readonly IColectivosService _colectivosService;

        #endregion

        #region Constructor

        public ColectivoController(IColectivosService colectivosService)
        {
            _colectivosService = colectivosService;
        }

        #endregion

        #region Endpoints

        /// <summary>
        /// A partir de una coordenada se obtiene el numero de fila y columna de la matriz
        /// </summary>
        /// <param name="coordenada"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "latitude": -37.291009,
        ///        "longitude": -59.156836
        ///     }
        ///
        /// </remarks>
        [HttpPost("ObtenerIndicesMatriz")]
        public ActionResult ObtenerIndicesMatriz(Coordenada coordenada)
        {
            try
            {
                var fila = _colectivosService.ObtenerIndiceFilaLatitud(coordenada.Latitude);
                var col = _colectivosService.ObtenerIndiceColumnaLongitud(coordenada.Longitude);

                return Ok($"fila: {fila} + columna: {col}");
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        #endregion
    }
}