namespace Domain.Entities.Dataset
{
    public class DatasetHistorico
    {
        public int Linea { get; set; }

        public int Id { get; set; }

        public long Tiempo_Request { get; set; }

        public long Tiempo_Colectivo { get; set; }

        public double Velocidad { get; set; }

        public double Latitud { get; set; }

        public double Longitud { get; set; }

    }
}
