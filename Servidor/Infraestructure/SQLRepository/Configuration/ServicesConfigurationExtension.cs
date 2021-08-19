using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SQLRepository.Contexts;

namespace SQLRepository.Configuration
{
    public static class ServicesConfigurationExtension
    {
        public static void AddCustomServices(this IServiceCollection services)
        { 
            services.AddDbContext<RegresionMultipleContext>(options => options.UseSqlite(@"Data Source=RegresionMultiple.db"));
            services.AddDbContext<DiccionarioContext>(options => options.UseSqlite(@"Data Source=Diccionarios.db")); ;
        }
    }
}
