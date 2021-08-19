using Domain.Interfaces;
using FileRepository.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Regresion.Interfaces;
using Regresion.Services;
using Services.Interfaces;
using Services.Services;
using SQLRepository.Configuration;
using SQLRepository.Repositories;
using System;
using System.IO;
using System.Reflection;

namespace PredictorTiempoColectivo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();

            #region Dependency Injection

            services.AddTransient<ICalculadorDeTiempoService, CalculadorDeTiempoService>();
            services.AddTransient<IColectivosService, ColectivosService>();
            services.AddTransient<IDiccionarioService, DiccionarioService>();
            services.AddTransient<IFranjasHorariasService, FranjasHorariasService>();
            services.AddTransient<IRecorridoBaseService, RecorridoBaseService>();
            services.AddTransient<IGeneradorDatasetService, GeneradorDatasetService>();
            services.AddTransient<IAnalizadorRecorridosService, AnalizadorRecorridosService>();
            services.AddTransient<ICalculadorDistanciaService, CalculadorDistanciaService>();

            services.AddTransient<IRegresionDiferenciaCeldasService, RegresionDiferenciaCeldasService>();

            services.AddTransient<IFranjaHorariaRepository, FranjaHorariaRepository>();
            services.AddTransient<IDiccionarioRepository, DiccionarioRepository>();
            services.AddTransient<IDatasetRepository, DatasetRepository>();
            services.AddTransient<IRecorridosRepository, RecorridosRepository>();
            services.AddTransient<IParadasRepository, ParadasRepository>();
            services.AddTransient<IRegresionRepository, RegresionRepository>();

            #endregion

            #region Swagger

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Predictor de tiempo API",
                    Description = "Proyecto Final realizado por Mauro Longhi y Martin Bianco"
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            #endregion

            #region Database

            services.AddCustomServices();

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            #region Swagger

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1.0");
            });

            #endregion

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
