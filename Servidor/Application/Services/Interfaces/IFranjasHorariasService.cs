using Domain.DTOs;
using System;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface IFranjasHorariasService
    {
        List<FranjaHorariaDTO> ObtenerFranjas();

        FranjaHorariaDTO ObtenerFranjaPorFecha(DateTime fecha);
    }
}