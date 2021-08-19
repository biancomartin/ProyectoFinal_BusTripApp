using Domain.Entities;

namespace Api.InputEntities
{
    public class ParCoordenadasInput
    {
        public Coordenada Origen { get; set; }

        public Coordenada Destino { get; set; }
    }
}
