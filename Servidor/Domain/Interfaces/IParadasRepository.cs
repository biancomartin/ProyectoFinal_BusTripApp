using Domain.DTOs;
using System.Collections.Generic;

namespace Domain.Interfaces
{
    public interface IParadasRepository
    {
        List<ParadaDTO> ObtenerParadas();

        void Insertar(ParadaDTO parada);

        List<ParadaDTO> ObtenerParadasPorRecorridoId(int recorridoId);

        void RemoverParadas();

    }
}
