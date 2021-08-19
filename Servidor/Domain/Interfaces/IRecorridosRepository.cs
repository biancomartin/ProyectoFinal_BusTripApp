using Domain.DTOs;
using System.Collections.Generic;

namespace Domain.Interfaces
{
    public interface IRecorridosRepository
    {
        List<RecorridosDTO> ObtenerRecorridos();

        void InsertarRecorrido(RecorridosDTO recorrido);

        RecorridosDTO ObtenerRecorridoPorLineaYTrayecto(int linea, int trayecto);

    }
}
