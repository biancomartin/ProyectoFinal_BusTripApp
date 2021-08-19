namespace Domain.DTOs
{
    public class DiccionarioDTO
    {
        public int Id { get; set; }

        public int RecorridoId { get; set; }

        public int FranjaId { get; set; }

        public int PuntoOrigen { get; set; }

        public int PuntoDestino { get; set; }

        public double Tiempo { get; set; }

        public int CantidadDeMuestras { get; set; }

        public int Unidad { get; set; }
    }
}
