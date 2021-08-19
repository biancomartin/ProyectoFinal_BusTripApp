namespace Domain.DTOs
{
    public class FranjaHorariaDTO
    {
        public int Id { get; set; }

        public int HoraInicio { get; set; }

        public int HoraFin { get; set; }

        public bool FinDeSemana { get; set; }
    }
}
