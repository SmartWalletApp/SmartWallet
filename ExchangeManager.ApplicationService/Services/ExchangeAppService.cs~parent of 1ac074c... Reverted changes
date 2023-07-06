using ExchangeManager.ApplicationService.Contracts;
using ExchangeManager.DomainModel.DataModels;
using ExchangeManager.DomainModel.RepositoryContracts;
using System.Text;

namespace ExchangeManager.ApplicationService.Services
{
    public class ExchangeAppService : IExchangeAppService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExchangeAppService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            return await _unitOfWork.CustomerRepository.GetAll();
        }

        public async Task<Customer> GetCustomer(int id)
        {
            return await _unitOfWork.CustomerRepository.GetByID(id);
        }

        public async Task<Customer> InsertCustomer(Customer customer)
        {
            // Currently bcrypt will use Cost 11 (2048 iteratios) by default, which is around 140ms in debug using a Ryzen 5 3600X and DDR4 3200MHz RAM.
            customer.Password = BCrypt.Net.BCrypt.HashPassword(customer.Password);

            var coins = await _unitOfWork.CoinRepository.GetAll();

            customer.Wallets = new List<Wallet>();

            foreach (var coin in coins)
            {
                var wallet = new Wallet
                {
                    Balance = 0,
                    Coin = coin,
                    BalanceHistory = new List<BalanceHistory>()
                };
                customer.Wallets.Add(wallet);
            }
            var output = await _unitOfWork.CustomerRepository.Insert(customer);
            _unitOfWork.Save();
            return output;
        }

        public async Task<Customer> DeleteCustomer(int id)
        {
            var output = await _unitOfWork.CustomerRepository.Delete(id);
            _unitOfWork.Save();
            return output;
        }

        public async Task<Customer> UpdateCustomer(Customer newCustomer)
        {
            var output = await _unitOfWork.CustomerRepository.Update(newCustomer);
            _unitOfWork.Save();
            return output;
        }

        public Task RestoreDB()
        {
            _unitOfWork.EnsureDeleted();
            _unitOfWork.EnsureCreated();
            _unitOfWork.CoinRepository.Insert(new Coin { Name = "USD", BuyValue = 0, SellValue = 0 });
            _unitOfWork.CoinRepository.Insert(new Coin { Name = "EUR", BuyValue = 0, SellValue = 0 });
            _unitOfWork.CoinRepository.Insert(new Coin { Name = "BTC", BuyValue = 0, SellValue = 0 });
            _unitOfWork.Save();
            return Task.CompletedTask;
        }

    }
}