using Domain.Diccionario;

namespace Services.Interfaces
{
    public interface IDiccionarioService
    {
        DiccionarioComplejo ObtenerDiccionarioComplejoPorFranja(int franjaId, int trayecto, int linea, int? unidadId);

        void GenerarDiccionarios();
    }
}