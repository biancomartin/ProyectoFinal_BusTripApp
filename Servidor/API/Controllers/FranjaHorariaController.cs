using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System;
using System.Net;

namespace ColectivosApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FranjaHorariaController : ControllerBase
    {
        #region Fields

        private readonly IFranjasHorariasService _franjasHorariasService;

        #endregion

        #region Constructor

        public FranjaHorariaController(IFranjasHorariasService franjasHorariasService)
        {
            this._franjasHorariasService = franjasHorariasService;
        }

        #endregion

        #region Endpoints

        /// <summary>
        /// Obtiene todas las franjas horarias que se tienen en cuenta
        /// </summary>
        /// <response code="200">Tiempo correspondiente</response>
        /// <response code="500">Error interno</response>
        /// <returns></returns>
        [HttpGet("ObtenerFranjas")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult ObtenerFranjas()
        {
            try
            {
                var franjas = _franjasHorariasService.ObtenerFranjas();
                return Ok(franjas);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        #endregion

    }
}