using Domain.DTOs;
using System.Collections.Generic;

namespace SQLRepository.Data
{
    public static class FranjasHorariasAConsiderar
    {

        public static List<FranjaHorariaDTO> franjaHorarias = new List<FranjaHorariaDTO>()
        {
            new FranjaHorariaDTO(){Id=1, FinDeSemana=false, HoraInicio=0, HoraFin=5},
            new FranjaHorariaDTO(){Id=2, FinDeSemana=false, HoraInicio=5, HoraFin=7},
            new FranjaHorariaDTO(){Id=3, FinDeSemana=false, HoraInicio=7, HoraFin=9},
            new FranjaHorariaDTO(){Id=4, FinDeSemana=false, HoraInicio=9, HoraFin=11},
            new FranjaHorariaDTO(){Id=5, FinDeSemana=false, HoraInicio=11, HoraFin=13},
            new FranjaHorariaDTO(){Id=6, FinDeSemana=false, HoraInicio=13, HoraFin=15},
            new FranjaHorariaDTO(){Id=7, FinDeSemana=false, HoraInicio=15, HoraFin=17},
            new FranjaHorariaDTO(){Id=8, FinDeSemana=false, HoraInicio=17, HoraFin=19},
            new FranjaHorariaDTO(){Id=9, FinDeSemana=false, HoraInicio=19, HoraFin=21},
            new FranjaHorariaDTO(){Id=10, FinDeSemana=false, HoraInicio=21, HoraFin=24},

            new FranjaHorariaDTO(){Id=11, FinDeSemana=true, HoraInicio=0, HoraFin=5},
            new FranjaHorariaDTO(){Id=12, FinDeSemana=true, HoraInicio=5, HoraFin=12},
            new FranjaHorariaDTO(){Id=13, FinDeSemana=true, HoraInicio=12, HoraFin=17},
            new FranjaHorariaDTO(){Id=14, FinDeSemana=true, HoraInicio=17, HoraFin=24}
        };

    }
}
