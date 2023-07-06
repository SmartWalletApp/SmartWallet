using SmartWallet.DomainModel.RepositoryContracts;

namespace SmartWallet.DomainModel.Persistence
{
    public interface IWalletRepository<T> : ISmartWalletRepository<T> where T : class
    {
    }
}