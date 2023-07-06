using SmartWallet.Infrastructure.DataModels;

namespace SmartWallet.ApplicationService.Contracts
{
    public interface ISmartWalletAppService
    {
        public Task<IEnumerable<Customer>> GetCustomers();

        public Task<Customer> GetCustomer(int id);

        public Task<Customer> InsertCustomer(Customer customer);

        public Task<Customer> DeleteCustomer(int id);

        public Task<Customer> UpdateCustomer(Customer newCustomer);

        public Task RestoreDB();

        public Task<Customer> VerifyCustomerLogin(string givenEmail, string givenPassword);
    }
}
