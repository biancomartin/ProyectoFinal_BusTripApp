using Domain.Dataset;
using Domain.Interfaces;
using MathNet.Numerics.Statistics;
using Microsoft.ML;
using Microsoft.Extensions.Logging;
using Regresion.Entities;
using Regresion.Interfaces;
using Regresion.Mappers;
using System;
using System.Linq;
using static Microsoft.ML.DataOperationsCatalog;
using System.Collections.Generic;

namespace Regresion.Services
{
    public class RegresionDiferenciaCeldasService : IRegresionDiferenciaCeldasService
    {

        #region Fields

        private readonly IRegresionRepository _regresionRepository;
        private readonly ILogger<RegresionDiferenciaCeldasService> _logger;
        private readonly ITransformer model;
        private readonly IDataView dataView;
        private readonly MLContext mlContext;
        private readonly TrainTestData trainTestData;
        private readonly List<DatasetModel> dataset = new List<DatasetModel>();

        #endregion

        #region Constructor

        public RegresionDiferenciaCeldasService(IRegresionRepository regresionRepository, ILogger<RegresionDiferenciaCeldasService> logger)
        {
            _regresionRepository = regresionRepository;
            _logger = logger;

            dataset = _regresionRepository.ObtenerDatos().Select(x => new DatasetModel
            {
                DiferenciaCeldas = x.DiferenciaCeldas,
                FranjaHorariaId = x.FranjaHorariaId,
                LineaId = x.LineaId,
                RecorridoId = x.RecorridoId,
                Tiempo = x.Tiempo,
                UnidadId = x.UnidadId
            }).ToList();
            mlContext = new MLContext(seed: 0);
            dataView = mlContext.Data.LoadFromEnumerable<DatasetModel>(dataset);
            trainTestData = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.25);
            var pipeline =
                mlContext.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: "Tiempo")
                .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "Unidad", inputColumnName: "UnidadId"))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "FranjaHoraria", inputColumnName: "FranjaHorariaId"))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "Recorrido", inputColumnName: "RecorridoId"))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "Linea", inputColumnName: "LineaId"))
                .Append(mlContext.Transforms.Concatenate("Features", "DiferenciaCeldas", "Unidad", "FranjaHoraria", "Recorrido", "Linea"))
                .Append(mlContext.Regression.Trainers.FastTree());

            model = pipeline.Fit(trainTestData.TrainSet);
        }

        #endregion

        #region Public Methods

        public RegresionMetricas ObtenerMetricas()
        {
            var predictions = model.Transform(trainTestData.TestSet);
            var metrics = mlContext.Regression.Evaluate(predictions, "Label", "Score");
            var correlation = Correlation.Pearson(dataset.Select(x => Convert.ToDouble(x.Tiempo)), dataset.Select(x => Convert.ToDouble(x.DiferenciaCeldas)));
            return RegressionMetricsMapper.ObtenerRegresionMetricas(metrics, correlation, 0);
        }

        public RegresionMetricas PredecirValor(DatasetModel datasetModel)
        {
            try
            {
                var predictions = model.Transform(trainTestData.TestSet);
                var metrics = mlContext.Regression.Evaluate(predictions, "Label", "Score");
                var correlation = Correlation.Pearson(dataset.Select(x => Convert.ToDouble(x.Tiempo)), dataset.Select(x => Convert.ToDouble(x.DiferenciaCeldas)));

                var tiempo = PredecirTiempo(datasetModel);
                return RegressionMetricsMapper.ObtenerRegresionMetricas(metrics, 0, tiempo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al calcular el tiempo con regresion");
            }
        }

        #endregion

        #region Private Methods

        private double PredecirTiempo(DatasetModel input)
        {
            try
            {
                var funcionPrediccion = mlContext.Model.CreatePredictionEngine<DatasetModel, DatasetPrediction>(model);
                var prediccion = funcionPrediccion.Predict(input);
                return prediccion.Tiempo;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al predecir el tiempo con regresion");
            }
        }

        #endregion
    }
}
