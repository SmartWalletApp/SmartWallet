namespace SmartWallet.DomainModel.RepositoryContracts
{
    public interface ICustomerRepository<T> : ISmartWalletRepository<T> where T : class
    {
        public Task<T> GetByEmail(string email);
    }
}