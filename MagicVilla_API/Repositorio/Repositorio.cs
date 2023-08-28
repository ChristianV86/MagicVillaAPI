using MagicVilla_API.Datos;
using MagicVilla_API.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;

namespace MagicVilla_API.Repositorio
{
    public class Repositorio<T> : IRepositorio<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> dbSet;

        public Repositorio(ApplicationDbContext context )
        {
            _context = context;
            this.dbSet = _context.Set<T>();
        }

        public async Task Crear(T entidad)
        {
            await dbSet.AddAsync( entidad );
            await Grabar();
        }

        public async Task Grabar()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<T> Obtener(Expression<Func<T, bool>> filtro = null, bool tracked = true)
        {
            IQueryable<T> query = dbSet;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }
            if(filtro != null)
            {
                query = query.Where( filtro );
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> ObtenerTodos(Expression<Func<T, bool>>? filtro = null)
        {
            IQueryable<T> query = dbSet;
            if (filtro != null)
            {
                query = query.Where(filtro);
            }
            return await query.ToListAsync();
        }

        public async Task Remover(T entidad)
        {
            dbSet.Remove(entidad);
            await Grabar();
        }

        private void SetCreationAndUpdateDates(T entidad)
        {
            var now = DateTime.Now;
            var entidadTipo = typeof(T);

            entidadTipo.GetProperty("FechaCreacion")?.SetValue(entidad, now);
            entidadTipo.GetProperty("FechaActualizacion")?.SetValue(entidad, now);
        }

        private void SetUpdateDate(T entidad)
        {
            var now = DateTime.Now;
            var entidadTipo = typeof(T);

            entidadTipo.GetProperty("FechaActualizacion")?.SetValue(entidad, now);
        }
    }
}
