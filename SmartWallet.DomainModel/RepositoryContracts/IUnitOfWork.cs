using SmartWallet.DomainModel.Entities;
using SmartWallet.DomainModel.Persistence;

namespace SmartWallet.DomainModel.RepositoryContracts
{
    public interface IUnitOfWork
    {
        void Save();
        void EnsureDeleted();
        void EnsureCreated();
    }
}
