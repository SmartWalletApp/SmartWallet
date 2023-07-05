using ExchangeManager.DomainModel.RepositoryContracts;
using ExchangeManager.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ExchangeManager.Infrastructure.Repositories
{
    public class ExchangeRepository<T> : IExchangeRepository<T>, IAsyncDisposable where T : class
    {
        private readonly ExchangeManagerDbContext _context;

        public ExchangeRepository(ExchangeManagerDbContext context)
        {
            _context = context;
        }
        protected DbSet<T> EntitySet { get { return _context.Set<T>(); } }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await EntitySet.ToListAsync();
        }

        public async Task<T> GetByID(int Id)
        {
            return await EntitySet.FindAsync(Id);
        }

        public async Task<T> Insert(T entity)
        {
            var entryAdded = await EntitySet.AddAsync(entity);
            return entryAdded.Entity;
        }

        public async Task<T> Delete(int ID)
        {
            var entryToDelete = await EntitySet.FindAsync(ID);
            var entryRemoved = EntitySet.Remove(entryToDelete);
            return entryRemoved.Entity;
        }

        public async Task<T> Update(T entity)
        {
            var entryUpdated = EntitySet.Update(entity);
            return entryUpdated.Entity;
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
            GC.SuppressFinalize(this);
        }
    }
}