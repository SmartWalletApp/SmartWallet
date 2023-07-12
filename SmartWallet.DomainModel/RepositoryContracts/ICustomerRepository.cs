using SmartWallet.DomainModel.Entities.Response;

namespace SmartWallet.DomainModel.RepositoryContracts
{
    public interface ICustomerRepository<T> : ISmartWalletRepository<T> where T : class
    {
        public Task<T> GetByEmail(string email);
        public Task<T> GetByID(int Id);
        public Task<T> UpdateAsync(T t);
    }
}