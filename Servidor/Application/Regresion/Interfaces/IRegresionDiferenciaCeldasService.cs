using Domain.Dataset;
using Regresion.Entities;

namespace Regresion.Interfaces
{
    public interface IRegresionDiferenciaCeldasService
    {
        RegresionMetricas PredecirValor(DatasetModel datasetModel);

        RegresionMetricas ObtenerMetricas();

    }
}
