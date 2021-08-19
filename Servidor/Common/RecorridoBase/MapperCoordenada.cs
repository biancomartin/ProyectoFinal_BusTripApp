using Domain.DTOs;
using Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Helper.RecorridoBase
{
    public static class MapperCoordenada
    {

        public static List<Coordenada> ObtenerCoordenadas(List<ParadaDTO> paradas)
        {
            var response = new List<Coordenada>();
            paradas.OrderBy(x => x.Orden);
            foreach(var parada in paradas)
            {
                response.Add(new Coordenada()
                {
                    Latitude = parada.Latitud,
                    Longitude = parada.Longitud
                });
            }

            return response;
        }
    }
}
