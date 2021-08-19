using Domain.Dataset;
using Microsoft.EntityFrameworkCore;

namespace SQLRepository.Contexts
{
    public class RegresionMultipleContext : DbContext
    {
        public RegresionMultipleContext(DbContextOptions<RegresionMultipleContext> options)
            : base(options) 
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RegresionMultiple>().Property(x => x.Id).ValueGeneratedOnAdd();
        }

        public DbSet<RegresionMultiple> RegresionMultipleHistorico { get; set; }
    }
}
