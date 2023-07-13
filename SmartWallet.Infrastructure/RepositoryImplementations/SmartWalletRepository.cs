using SmartWallet.DomainModel.RepositoryContracts;
using SmartWallet.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace SmartWallet.Infrastructure.RepositoryImplementations
{
    public class SmartWalletRepository<T> : ISmartWalletRepository<T>, IAsyncDisposable where T : class
    {
        protected readonly SmartWalletDbContext _context;
        protected DbSet<T> EntitySet;
        protected readonly IMapper _mapper;

        public SmartWalletRepository(SmartWalletDbContext context, IMapper mapper)
        {
            _context = context;
            EntitySet = _context.Set<T>();
            _mapper = mapper;
        }

        public virtual async Task<T> Insert(T entity)
        {
            var entryAdded = await EntitySet.AddAsync(entity);
            return entryAdded.Entity;
        }

        public virtual async Task<T> Delete(int ID)
        {
            var entryToDelete = await EntitySet.FindAsync(ID);
            if (entryToDelete != null)
            {
                var entryRemoved = EntitySet.Remove(entryToDelete);
                return entryRemoved.Entity;
            }
            throw new InvalidOperationException();
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
