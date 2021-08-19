using Api.InputEntities;
using Helper.Functions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DistanciaController : ControllerBase
    {
        /// <summary>
        /// A partir de dos coordenadas, retorna la distancia en metros usando Haversine
        /// </summary>
        /// <param name="coordenadas"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///         "Origen": {
        ///             "latitude": -37.328487,
        ///             "longitude": -59.138491
        ///         },
        ///         "Destino": {
        ///             "latitude": -37.327546,
        ///             "longitude": -59.136010
        ///         }
        ///     }
        ///
        /// </remarks>
        [HttpPost]
        public ActionResult ObtenerDistanciaEntreCoordenadasHaversine([FromBody] ParCoordenadasInput coordenadas)
        {
            try
            {
                Haversine haversine = new Haversine();
                var distance = haversine.Distance(coordenadas.Origen, coordenadas.Destino, DistanceType.Kilometers) * 1000;
                return Ok(distance);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
