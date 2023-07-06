using SmartWallet.DomainModel.RepositoryContracts;

namespace SmartWallet.DomainModel.Persistence
{
    public interface ICustomerRepository<T> : ISmartWalletRepository<T> where T : class
    {
    }
}