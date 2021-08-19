using Domain.Entities;
using System.Collections.Generic;

namespace Domain.RecorridoBase
{
    public class RecorridoBasePorRecorrido
    {
        public int Trayecto { get; set; }

        public int Linea { get; set; }

        public IEnumerable<Coordenada> Coordenadas { get; set; }

    }
}
