using Domain.Entities;
using Domain.Interfaces;
using Services.Interfaces;
using System.Linq;

namespace Services.Services
{
    public class GeneradorDatasetService : IGeneradorDatasetService
    {
        #region Fields

        private readonly IDiccionarioRepository _diccionarioRepository;
        private readonly IRecorridosRepository _recorridosRepository;
        private readonly IDatasetRepository _datasetRepository;

        #endregion

        #region Constructor

        public GeneradorDatasetService(IDiccionarioRepository diccionarioRepository,
                                        IRecorridosRepository recorridosRepository,
                                        IDatasetRepository datasetRepository)
        {
            _diccionarioRepository = diccionarioRepository;
            _recorridosRepository = recorridosRepository;
            _datasetRepository = datasetRepository;
        }

        #endregion

        #region Public Methods

        public void GenerarDatasetDiferenciaDeCeldas()
        {
            var diccionario = _diccionarioRepository.ObtenerDiccionarios();
            var recorridos = _recorridosRepository.ObtenerRecorridos();
            var dataset = diccionario.Select(x => new DiferenciaDeCeldas
            {
                LineaId = recorridos.FirstOrDefault(y => y.Id == x.RecorridoId).Linea.ToString(),
                Tiempo = (float)x.Tiempo,
                RecorridoId = x.RecorridoId.ToString(),
                FranjaHorariaId = x.FranjaId.ToString(),
                UnidadId = x.Unidad.ToString(),
                DiferenciaCeldas = x.PuntoDestino - x.PuntoOrigen
            }).ToList();
            _datasetRepository.GenerarArchivoJsonDiferenciaCeldas(dataset);
        }

        #endregion

    }
}