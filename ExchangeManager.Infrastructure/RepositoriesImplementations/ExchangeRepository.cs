using ExchangeManager.Data;
using ExchangeManager.DomainModel.RepositoryContracts;
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
            EntitySet.AddAsync(entity);
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

        public async Task Save()
        {
            context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}