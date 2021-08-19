using Domain.Entities;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface IAnalizadorRecorridosService
    {
        List<RecorridoMasCercano> ObtenerParadasCercanas(Coordenada coordenadaOrigen, Coordenada coordenadaDestino, List<int> lineas);
    }
}
