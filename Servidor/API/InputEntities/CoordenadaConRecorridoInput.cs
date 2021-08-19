using Domain.Entities;

namespace ColectivosApi.InputEntities
{
    public class CoordenadaConRecorridoInput
    {
        public Coordenada Coordenada { get; set; }

        public int Linea { get; set; }

        public int Trayecto { get; set; }

    }
}
