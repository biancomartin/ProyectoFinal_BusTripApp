using Domain.DTOs;
using System.Collections.Generic;

namespace SQLRepository.Data
{
    public static class RecorridosAConsiderar
    {

        public static List<RecorridosDTO> recorridos = new List<RecorridosDTO>()
        {
            new RecorridosDTO(){Id = 1, Base = 1, Linea = 500},
            new RecorridosDTO(){Id = 2, Base = 2, Linea = 500},

            new RecorridosDTO(){Id = 3, Base = 1, Linea = 501},
            new RecorridosDTO(){Id = 4, Base = 2, Linea = 501},
            
            new RecorridosDTO(){Id = 5, Base = 1, Linea = 502},
            new RecorridosDTO(){Id = 6, Base = 2, Linea = 502},
            new RecorridosDTO(){Id = 7, Base = 3, Linea = 502},

            new RecorridosDTO(){Id = 8, Base = 1, Linea = 503},
            new RecorridosDTO(){Id = 9, Base = 2, Linea = 503},
            new RecorridosDTO(){Id = 10, Base = 3, Linea = 503},
            new RecorridosDTO(){Id = 11, Base = 4, Linea = 503},

            new RecorridosDTO(){Id = 12, Base = 1, Linea = 504},
            new RecorridosDTO(){Id = 13, Base = 2, Linea = 504},

            new RecorridosDTO(){Id = 14, Base = 1, Linea = 505},
            new RecorridosDTO(){Id = 15, Base = 2, Linea = 505}
        };

    }
}
