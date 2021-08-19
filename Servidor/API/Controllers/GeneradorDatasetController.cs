using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System;
using System.Net;

namespace ColectivosApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GeneradorDatasetController : ControllerBase
    {
        #region Fields

        private readonly IGeneradorDatasetService _generadorDatasetService;
        private readonly IDiccionarioService _diccionarioService;

        #endregion

        #region Constructor

        public GeneradorDatasetController(IGeneradorDatasetService generadorDatasetService, IDiccionarioService diccionarioService)
        {
            _generadorDatasetService = generadorDatasetService;
            _diccionarioService = diccionarioService;
        }

        #endregion

        #region Endpoints

        /// <summary>
        /// Exporta el dataset de diferencia de celdas a un archivo json
        /// </summary>
        /// <returns></returns>
        [HttpPost("GenerarDatasetDiferenciaCeldas")]
        public ActionResult GenerarDatasetDiferenciaDeCeldas()
        {
            try
            {
                _generadorDatasetService.GenerarDatasetDiferenciaDeCeldas();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// En caso de haber nueva informacion (incluir en la carpeta API\Dataset) se deben generar los diccionarios para tener en cuenta en los calculos
        /// </summary>
        /// <returns></returns>
        [HttpPost("GenerarDiccionario")]
        public ActionResult GenerarDiccionario()
        {
            try
            {
                _diccionarioService.GenerarDiccionarios();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        #endregion

    }
}