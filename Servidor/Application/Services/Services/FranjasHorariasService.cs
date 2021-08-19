using Domain.DTOs;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Services.Interfaces;
using System;
using System.Collections.Generic;

namespace Services.Services
{
    public class FranjasHorariasService : IFranjasHorariasService
    {
        #region Fields

        private readonly IFranjaHorariaRepository _franjaHorariaRepository;
        private readonly ILogger<FranjasHorariasService> _logger;

        #endregion

        #region Constructor

        public FranjasHorariasService(IFranjaHorariaRepository franjaHorariaRepository, ILogger<FranjasHorariasService> logger)
        {
            _franjaHorariaRepository = franjaHorariaRepository;
            _logger = logger;
        }

        #endregion

        #region Public Methods

        public List<FranjaHorariaDTO> ObtenerFranjas()
        {
            try
            {
                return _franjaHorariaRepository.ObtenerFranjaHorarias();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al intentar obtener las franjas", ex.InnerException);
            }
        }

        public FranjaHorariaDTO ObtenerFranjaPorFecha(DateTime fecha)
        {
            try
            {
                return _franjaHorariaRepository.ObtenerFranjaPorFecha(fecha);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al intentar obtener las franjas por fecha", ex.InnerException);
            }
        }

        #endregion

    }
}