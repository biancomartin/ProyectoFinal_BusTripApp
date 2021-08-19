using Domain.Entities;
using Domain.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;
using Domain.Entities.Dataset;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace FileRepository.Repositories
{
    public class DatasetRepository : IDatasetRepository
    {

        private readonly IHostingEnvironment _appEnvironment;

        public DatasetRepository(IHostingEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }

        public List<DatasetHistorico> ObtenerColectivosHistoricos()
        {
            List<DatasetHistorico> response = new List<DatasetHistorico>();
            string _dataPath = Path.Combine(_appEnvironment.ContentRootPath, @"Dataset/");

            foreach (string fileName in Directory.GetFiles(_dataPath, "*.json"))
            {
                using (StreamReader r = new StreamReader(fileName))
                {
                    string json = r.ReadToEnd();
                    response.AddRange(JsonConvert.DeserializeObject<List<DatasetHistorico>>(json));
                }
            }
            return response;
        }

        public void GenerarArchivoJsonDiferenciaCeldas(List<DiferenciaDeCeldas> dataset)
        {
            string JSONresult = JsonConvert.SerializeObject(dataset);
            using (var tw = new StreamWriter("DiferenciaCeldas.json", true))
            {
                tw.WriteLine(JSONresult.ToString());
                tw.Close();
            }
        }
    }
}