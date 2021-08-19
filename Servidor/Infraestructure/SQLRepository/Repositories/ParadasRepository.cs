using Domain.DTOs;
using Domain.Interfaces;
using SQLRepository.Contexts;
using System.Collections.Generic;
using System.Linq;

namespace SQLRepository.Repositories
{
    public class ParadasRepository : IParadasRepository
    {
        private readonly DiccionarioContext _context;

        public ParadasRepository(DiccionarioContext context)
        {
            _context = context;
        }

        #region Methods

        public List<ParadaDTO> ObtenerParadas()
        {
            return _context.Paradas.ToList();
        }

        public void Insertar(ParadaDTO parada)
        {
            _context.Paradas.Add(parada);
            _context.SaveChanges();
        }

        public List<ParadaDTO> ObtenerParadasPorRecorridoId(int recorridoId)
        {
            return _context.Paradas
                        .Where(x => x.RecorridoId == recorridoId)
                        .OrderBy(x => x.Orden).ToList();
        }

        public void RemoverParadas()
        {
            var paradasParaBorrar = ObtenerParadas();
            _context.Paradas.RemoveRange(paradasParaBorrar.ToArray());
            _context.SaveChanges();
        }

        #endregion
    }
}
