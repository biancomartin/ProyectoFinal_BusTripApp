using Domain.DTOs;
using System;
using System.Collections.Generic;

namespace Domain.Interfaces
{
    public interface IFranjaHorariaRepository
    {
        List<FranjaHorariaDTO> ObtenerFranjaHorarias();

        void Insertar(FranjaHorariaDTO franjaHoraria);

        FranjaHorariaDTO ObtenerFranjaPorFecha(DateTime datetime);

        void RemoverFranjas();
    }
}
