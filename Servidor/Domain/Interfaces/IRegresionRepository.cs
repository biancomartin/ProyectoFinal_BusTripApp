using Domain.Dataset;
using System.Collections.Generic;

namespace Domain.Interfaces
{
    public interface IRegresionRepository
    {
        List<RegresionMultiple> ObtenerDatos();

        void Insertar(List<RegresionMultiple> regresion);

        void DeleteAll();
    }
}
