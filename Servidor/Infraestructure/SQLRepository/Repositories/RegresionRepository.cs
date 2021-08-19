using Domain.Dataset;
using Domain.Interfaces;
using SQLRepository.Contexts;
using System.Collections.Generic;
using System.Linq;

namespace SQLRepository.Repositories
{
    public class RegresionRepository : IRegresionRepository
    {
        private readonly RegresionMultipleContext _context;

        public RegresionRepository(RegresionMultipleContext context)
        {
            _context = context;
        }

        #region Methods

        public List<RegresionMultiple> ObtenerDatos()
        {
            return _context.RegresionMultipleHistorico.ToList();
        }

        public void Insertar(List<RegresionMultiple> regresion)
        {
            _context.RegresionMultipleHistorico.AddRange(regresion);
            _context.SaveChanges();
        }

        public void DeleteAll()
        {
            var datosParaRemover = ObtenerDatos();
            _context.RegresionMultipleHistorico.RemoveRange(datosParaRemover.ToArray());
            _context.SaveChanges();
        }

        #endregion

    }
}