using Domain.DTOs;
using Domain.Entities;
using Domain.RecorridoBase;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface IRecorridoBaseService
    {

        List<Coordenada> ObtenerRecorridoBasePorLineaYTrayecto(int linea, int trayecto);

        List<RecorridoBasePorRecorrido> ObtenerRecorridoBasePorLinea(int linea);

        Coordenada ObtenerParadaMasCercana(Coordenada coordenada, int linea, int trayecto);

        List<RecorridosDTO> ObtenerRecorridos();
    }
}