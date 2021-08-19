using Domain.DTOs;
using System.Collections.Generic;

namespace Domain.Interfaces
{
    public interface IDiccionarioRepository
    {
        List<DiccionarioDTO> ObtenerDiccionarios();

        void Insertar(DiccionarioDTO diccionario);

        List<DiccionarioDTO> ObtenerDiccionarioFiltrado(int recorridoId, int franjaId, int? unidadId);

        void RemoverDiccionario();
    }
}
