using SmartWallet.Infrastructure.DataModels;
using SmartWallet.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using SmartWallet.DomainModel.RepositoryContracts;

namespace SmartWallet.Infrastructure.RepositoryImplementations
{
    public class CustomerRepository : SmartWalletRepository<Customer>, ICustomerRepository<Customer>
    {
        public CustomerRepository(SmartWalletDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<Customer> GetByEmail(string email) =>
            await EntitySet
                .Where(c => c.Email == email)
                .Include(c => c.Wallets)
                    .ThenInclude(w => w.Coin)
                .Include(c => c.Wallets)
                    .ThenInclude(w => w.BalanceHistorics)
                .FirstOrDefaultAsync() ?? throw new InvalidOperationException($"Customer not found");

        public async Task<Customer> GetByID(int Id) =>
            await EntitySet
                .Where(c => c.Id == Id)
                .Include(c => c.Wallets)
                    .ThenInclude(w => w.Coin)
                .Include(c => c.Wallets)
                    .ThenInclude(w => w.BalanceHistorics)
                .FirstOrDefaultAsync() ?? throw new InvalidOperationException($"Customer not found");


        public async Task<Customer> UpdateAsync(Customer customer)
        {
            if (customer == null) throw new InvalidOperationException($"New customer is null");

            var existingCustomer = await GetByID(customer.Id);

            existingCustomer.Name = customer.Name;
            existingCustomer.Surname = customer.Surname;
            existingCustomer.Password = customer.Password;
            existingCustomer.SecurityGroup = customer.SecurityGroup;

            return existingCustomer;
        }
    }
}
