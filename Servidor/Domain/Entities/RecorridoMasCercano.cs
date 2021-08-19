using System.Collections.Generic;

namespace Domain.Entities
{
    public class RecorridoMasCercano
    {
        public Coordenada CoordenadaOrigen { get; set; }

        public Coordenada CoordenadaDestino { get; set; }

        public int Trayecto { get; set; }

        public int Linea { get; set; }

        public List<Coordenada> CoordenadasIntermedias { get; set; }
    }
}
