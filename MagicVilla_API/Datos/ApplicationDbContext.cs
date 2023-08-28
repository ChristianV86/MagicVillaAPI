using MagicVilla_API.Modelos;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MagicVilla_API.Datos
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {

        }
        public DbSet<Villa> Villas { get; set; }
        public DbSet<NumeroVilla> numeroVillas { get; set; }

        public override int SaveChanges()
        {
            var entradas = ChangeTracker.Entries();

            foreach (var entrada in entradas)
            {
                if(entrada.State == EntityState.Added)
                {
                    entrada.Property("FechaCreacion").CurrentValue = DateTime.Now;
                    entrada.Property("FechaActualizacion").CurrentValue = DateTime.Now;
                }
                if(entrada.State == EntityState.Modified)
                {
                    entrada.Property("FechaCreacion").IsModified = false;
                    entrada.Property("FechaActualizacion").CurrentValue = DateTime.Now;
                }
            }
            return base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa()
                {
                    Id = 1,
                    Nombre = "Premium vista a la Piscina",
                    Detalle = "Detalle de la Villa...",
                    ImagenUrl = "",
                    Ocupantes = 10,
                    MetrosCuadrados=200,
                    Tarifa=400,
                    Amenidad="",
                    FechaCreacion= DateTime.Now,
                    FechaActualizacion= DateTime.Now
                },
                 new Villa()
                 {
                     Id = 2,
                     Nombre = "Villa Real",
                     Detalle = "Detalle de la Villa...",
                     ImagenUrl = "",
                     Ocupantes = 5,
                     MetrosCuadrados = 100,
                     Tarifa = 200,
                     Amenidad = "",
                     FechaCreacion = DateTime.Now,
                     FechaActualizacion = DateTime.Now
                 }
            );
        }
    }
}
