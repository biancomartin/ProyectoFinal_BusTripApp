using Domain.Entities;
using System.Collections.Generic;

namespace Domain.RecorridoBase
{
    public class ParadaMasCercanas
    {
        public int Trayecto { get; set; }

        public int Linea { get; set; }

        public List<Coordenada> CoordenadasIntermedias { get; set; }
    }
}
