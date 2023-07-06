using ExchangeManager.Infrastructure.DataModels;

namespace ExchangeManager.ApplicationService.Contracts
{
    public interface IExchangeAppService
    {
        public Task<IEnumerable<Customer>> GetCustomers();

        public Task<Customer> GetCustomer(int id);

        public Task<Customer> InsertCustomer(Customer customer);

        public Task<Customer> DeleteCustomer(int id);

        public Task<Customer> UpdateCustomer(Customer newCustomer);

        public Task RestoreDB();
    }
}
