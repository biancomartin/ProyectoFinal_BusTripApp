using Domain.DTOs;
using Domain.Interfaces;
using SQLRepository.Contexts;
using System.Collections.Generic;
using System.Linq;

namespace SQLRepository.Repositories
{
    public class DiccionarioRepository : IDiccionarioRepository
    {
        private readonly DiccionarioContext _context;

        public DiccionarioRepository(DiccionarioContext context)
        {
            _context = context;
        }

        #region Methods

        public List<DiccionarioDTO> ObtenerDiccionarios()
        {
            return _context.Diccionarios.ToList();
        }

        public void Insertar(DiccionarioDTO diccionario)
        {
            _context.Diccionarios.Add(diccionario);
            _context.SaveChanges();
        }

        public List<DiccionarioDTO> ObtenerDiccionarioFiltrado(int recorridoId, int franjaId, int? unidadId)
        {
            var diccionarios = _context.Diccionarios.Where(x => x.RecorridoId == recorridoId && x.FranjaId == franjaId).ToList();
            if (unidadId != null)
            {
                diccionarios = diccionarios.Where(x => x.Unidad == unidadId).ToList();
            }
            return diccionarios;
        }

        public void RemoverDiccionario()
        {
            var diccionarios = ObtenerDiccionarios();
            _context.Diccionarios.RemoveRange(diccionarios);
            _context.SaveChanges();
        }

        #endregion

    }
}