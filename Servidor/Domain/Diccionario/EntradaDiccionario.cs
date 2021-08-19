using System.Collections.Generic;

namespace Domain.Diccionario
{
    public class EntradaDiccionario
    {
        public int UnidadId { get; set; }

        public int PuntoOrigen { get; set; }

        public int PuntoDestino { get; set; }

        public List<double> Segundos { get; set; }

        public int CantidadDeMuestras { get; set; }
    }
}
