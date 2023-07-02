using ExchangeManager.DomainModel.RepositoryContracts;
using ExchangeManager.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ExchangeManager.Infrastructure.Repositories
{
    public class ExchangeRepository<T> : IExchangeRepository<T> where T : class
    {
        private readonly ExchangeManagerDbContext context;

        public ExchangeRepository(ExchangeManagerDbContext context)
        {
            this.context = context;
        }
        protected DbSet<T> EntitySet
        {
            get
            {
                return context.Set<T>();
            }
        }


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
            await EntitySet.AddAsync(entity);
            await Save();
            return entity;
        }

        public async Task<T> Delete(int ID)
        {
            T entity = await EntitySet.FindAsync(ID);
            EntitySet.Remove(entity);
            await Save();
            return entity;

        }

        public async Task Update(T entity)
        {
            EntitySet.Entry(entity).State = EntityState.Modified;
            await Save();
        }

        public Task Save()
        {
            var rowsModified = context.SaveChanges();
            return rowsModified > 0 ? Task.CompletedTask : throw new Exception("No changes were made.");
        }

        private bool disposed = false;

        protected virtual void DisposeAsync(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.DisposeAsync();
                }
            }
            disposed = true;
        }

        public async void Dispose()
        {
            DisposeAsync(true);
            GC.SuppressFinalize(this);
        }
    }
}