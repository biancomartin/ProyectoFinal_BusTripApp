using Domain.DTOs;
using Domain.Interfaces;
using SQLRepository.Contexts;
using System.Collections.Generic;
using System.Linq;

namespace SQLRepository.Repositories
{
    public class RecorridosRepository : IRecorridosRepository
    {
        private readonly DiccionarioContext _context;

        public RecorridosRepository(DiccionarioContext context)
        {
            this._context = context;
        }

        #region Methods

        public List<RecorridosDTO> ObtenerRecorridos()
        {
            return _context.Recorridos.ToList();
        }

        public void InsertarRecorrido(RecorridosDTO recorrido)
        {
            _context.Recorridos.Add(recorrido);
            _context.SaveChanges();
        }

        public RecorridosDTO ObtenerRecorridoPorLineaYTrayecto(int linea, int trayecto)
        {
            var recorrido = _context.Recorridos.FirstOrDefault(x => x.Linea == linea && x.Base == trayecto);
            return recorrido;
        }

        #endregion

    }
}