using Domain.Entities;
using System.Collections.Generic;

namespace ColectivosApi.InputEntities
{
    public class CaminosAlternativosInput
    {
        public Coordenada CoordenadaOrigen { get; set; }

        public Coordenada CoordenadaDestino { get; set; }

        public List<int> Lineas { get; set; }

    }
}
