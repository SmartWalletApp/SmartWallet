using SmartWallet.DomainModel.Entities.Response;

namespace SmartWallet.DomainModel.RepositoryContracts
{
    public interface IBalanceHistoric<T> : ISmartWalletRepository<T> where T : class
    {
    }
}
