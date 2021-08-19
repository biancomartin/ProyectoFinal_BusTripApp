using Domain.DTOs;
using Microsoft.EntityFrameworkCore;
using SQLRepository.Data;

namespace SQLRepository.Contexts
{
    public class DiccionarioContext : DbContext
    {
        public DiccionarioContext(DbContextOptions<DiccionarioContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DiccionarioDTO>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<RecorridosDTO>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<FranjaHorariaDTO>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<ParadaDTO>().Property(x => x.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<RecorridosDTO>().HasData(
                RecorridosAConsiderar.recorridos
            );

            modelBuilder.Entity<FranjaHorariaDTO>().HasData(
                FranjasHorariasAConsiderar.franjaHorarias
            );
        }
        
        public DbSet<DiccionarioDTO> Diccionarios { get; set; }

        public DbSet<RecorridosDTO> Recorridos { get; set; }

        public DbSet<FranjaHorariaDTO> FranjaHorarias { get; set; }

        public DbSet<ParadaDTO> Paradas { get; set; }
    }
}
