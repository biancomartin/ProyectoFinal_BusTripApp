using Domain.DTOs;
using Domain.Interfaces;
using SQLRepository.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SQLRepository.Repositories
{
    public class FranjaHorariaRepository : IFranjaHorariaRepository
    {
        private readonly DiccionarioContext _context;

        public FranjaHorariaRepository(DiccionarioContext context)
        {
            _context = context;
        }

        #region Methods

        public List<FranjaHorariaDTO> ObtenerFranjaHorarias()
        {
            return _context.FranjaHorarias.ToList();
        }

        public void Insertar(FranjaHorariaDTO franjaHoraria)
        {
            _context.FranjaHorarias.Add(franjaHoraria);
            _context.SaveChanges();
        }

        public FranjaHorariaDTO ObtenerFranjaPorFecha(DateTime datetime)
        {
            bool diaDeSemana = ((int)datetime.DayOfWeek == 0 || (int)datetime.DayOfWeek == 6) ? true : false;
            return _context.FranjaHorarias
                        .Where(x => x.HoraInicio <= datetime.Hour && x.HoraFin >= datetime.Hour)
                        .Where(x => x.FinDeSemana == diaDeSemana)
                        .FirstOrDefault();
        }

        public void RemoverFranjas()
        {
            var franjasParaBorrar = ObtenerFranjaHorarias();
            _context.FranjaHorarias.RemoveRange(franjasParaBorrar.ToArray());
            _context.SaveChanges();
        }

        #endregion

    }
}