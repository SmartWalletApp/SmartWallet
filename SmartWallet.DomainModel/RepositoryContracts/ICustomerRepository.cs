using SmartWallet.DomainModel.Entities.Response;

namespace SmartWallet.DomainModel.RepositoryContracts
{
    public interface ICustomerRepository<T> : ISmartWalletRepository<T> where T : class
    {
        public Task<T> GetByID(int Id, bool GetBalanceHistorics = false);
        public Task<T> GetByEmail(string email, bool GetBalanceHistorics = false);
        public Task<T> UpdateAsync(T t);
    }
}