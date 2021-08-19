using Domain.Dataset;
using Microsoft.ML.Data;

namespace Regresion.Mappers
{
    public static class RegressionMetricsMapper
    {
        public static RegresionMetricas ObtenerRegresionMetricas(RegressionMetrics regressionMetrics, double correlation, double prediction)
        {
            return new RegresionMetricas
            {
                MeanAbsoluteError = regressionMetrics.MeanAbsoluteError,
                LossFunction = regressionMetrics.LossFunction,
                RSquared = regressionMetrics.RSquared,
                MeanSquaredError = regressionMetrics.MeanSquaredError,
                RootMeanSquaredError = regressionMetrics.RootMeanSquaredError,
                Correlation = correlation,
                Predicction = prediction
            };
        }
    }
}
