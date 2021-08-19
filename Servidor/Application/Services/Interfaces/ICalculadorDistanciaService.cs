using Domain.Entities;

namespace Services.Interfaces
{
    public interface ICalculadorDistanciaService
    {
        double CalcularDistancia(Coordenada origen, Coordenada destino, int linea, int trayecto);
    }
}
