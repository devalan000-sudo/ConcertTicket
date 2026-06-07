using System.Linq.Expressions;

namespace ConcertTicket.Application.Interfaces
{
    public interface IRepository<T> where T : class
    {
        //Lectura
        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        //Busqueda de filtros
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        //Escritura
        Task AddAsync(T entity);

        //Actualizacion y borrado
        void Update(T entity);
        void Delete(T entity);

        //Ejecutar en SQL y no en RAM
        IQueryable<T> AsQueryable();
    }
}
