using Domain.Entities;
using System;

namespace ColectivosApi.InputEntities
{
    public class CalcularTiempoInput
    {
        public Coordenada PosicionOrigen { get; set; }

        public Coordenada PosicionDestino { get; set; }

        public DateTime Fecha { get; set; }

        public int Trayecto { get; set; }

        public int LineaId { get; set; }

        public int? UnidadId { get; set; }

    }
}
