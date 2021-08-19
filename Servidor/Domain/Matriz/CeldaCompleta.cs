using Domain.Entities.Dataset;
using System.Collections.Generic;

namespace Domain.Matriz
{
    public class CeldaCompleta
    {
        public CeldaMatriz Celda { get; set; }

        public DatasetHistorico DatasetHistorico { get; set; }

        public Dictionary<int, int> PosicionesPorRecorrido { get; set; }
    }
}
