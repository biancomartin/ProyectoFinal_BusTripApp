using Domain.Entities;
using Domain.RecorridoBase;
using System.Collections.Generic;
using System.Linq;

namespace ColectivosApi.Mappers
{
    public static class ParadaMasCercanasMapper
    {

        public static ParadaMasCercanas ObtenerParadaMasCercanas(RecorridoMasCercano recorridoMasCercano)
        {
            return new ParadaMasCercanas()
            {
                CoordenadasIntermedias = recorridoMasCercano.CoordenadasIntermedias,
                Trayecto = recorridoMasCercano.Trayecto,
                Linea = recorridoMasCercano.Linea
            };
        }

        public static List<ParadaMasCercanas> ObtenerParadasMasCercanas(List<RecorridoMasCercano> recorridoMasCercano)
        {
            return recorridoMasCercano.Select(x => ObtenerParadaMasCercanas(x)).ToList();

        }
    }
}
