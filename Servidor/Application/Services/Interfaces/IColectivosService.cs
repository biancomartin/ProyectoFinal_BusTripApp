using Domain.DTOs;
using Domain.Entities.Dataset;
using Domain.Matriz;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface IColectivosService
    {
        List<double> ObtenerFilasMatriz();

        List<double> ObtenerColumnasMatriz();

        int ObtenerIndiceFilaLatitud(double value);

        int ObtenerIndiceColumnaLongitud(double value);

        int ObtenerIndiceDeRecorridoBase(int fila, int columna, int trayecto, int linea);

        List<CeldaMatriz> GenerarCeldasAPartirDeLineaYTrayecto(int linea, int trayecto);

        List<DatasetHistorico> ObtenerDatasetColectivosPorHorario(FranjaHorariaDTO franjaHoraria);
    }
}