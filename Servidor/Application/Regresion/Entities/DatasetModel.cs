using Microsoft.ML.Data;

namespace Regresion.Entities
{
    public class DatasetModel
    {
        [LoadColumn(1)] 
        public string UnidadId { get; set; }
        [LoadColumn(2)] 
        public string FranjaHorariaId { get; set; }
        [LoadColumn(3)] 
        public string RecorridoId { get; set; }
        [LoadColumn(4)] 
        public string LineaId { get; set; }
        [LoadColumn(5)] 
        public float Tiempo { get; set; }
        [LoadColumn(6)] 
        public float DiferenciaCeldas { get; set; }
    }


    public class DatasetPrediction
    {
        [ColumnName("Score")]
        public float Tiempo;
    }
}
