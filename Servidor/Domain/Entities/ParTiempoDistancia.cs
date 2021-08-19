namespace Domain.Entities
{
    public class ParTiempoDistancia
    {
        public double Tiempo { get; set; }

        public double Distancia { get; set; }

        public Coordenada CoordenadaOrigen { get; set; }

        public Coordenada CoordenadaDestino { get; set; }

        public int Linea { get; set; }

        public int Trayecto { get; set; }
    }
}
