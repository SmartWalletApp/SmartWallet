using SmartWallet.DomainModel.Entities.Response;

namespace SmartWallet.DomainModel.RepositoryContracts
{
    public interface ICustomerRepository<T> : ISmartWalletRepository<T> where T : class
    {
        public Task<T> UpdateAsync(T t);
        public Task<T> GetByEmail(string email);
    }
}