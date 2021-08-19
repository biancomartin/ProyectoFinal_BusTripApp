using Domain.Entities;
using Domain.Entities.Dataset;
using System.Collections.Generic;

namespace Domain.Interfaces
{
    public interface IDatasetRepository
    {
        void GenerarArchivoJsonDiferenciaCeldas(List<DiferenciaDeCeldas> dataset);

        List<DatasetHistorico> ObtenerColectivosHistoricos();
    }
}
