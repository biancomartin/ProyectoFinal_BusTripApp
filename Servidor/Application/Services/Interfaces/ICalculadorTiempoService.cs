using Domain.Dataset;
using Domain.Entities;
using System;

namespace Services.Interfaces
{
    public interface ICalculadorDeTiempoService
    {
        double ObtenerTiempoEntreCoordenadasComplejo(Coordenada posicionOrigen, Coordenada posicionDestino, DateTime fecha, int trayecto, int linea, int? unidadId);

        RegresionMetricas ObtenerTiempoPorRegresionDiferenciaDeCeldas(Coordenada posicionOrigen, Coordenada posicionDestino, DateTime fecha, int trayecto, int linea, int? unidadId);
    }
}