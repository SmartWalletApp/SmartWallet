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
        public async Task<Customer> GetByEmail(string email)
        {
            return await EntitySet.FirstOrDefaultAsync(x => x.Email == email) ?? throw new InvalidOperationException("Customer not found");
        }

        public override async Task<IEnumerable<Customer>> GetAll() =>
            await EntitySet.Include(c => c.Wallets)
                .ThenInclude(w => w.Coin)
                .Include(c => c.Wallets)
                //.ThenInclude(w => w.BalanceHistoric)
                .ToListAsync();

        public async Task<Customer> UpdateAsync(Customer customer)
        {
            var existingCustomer = await EntitySet.FirstOrDefaultAsync(c => c.Email == customer.Email);

            if(existingCustomer != null)
            {
                existingCustomer.Name = customer.Name;
                existingCustomer.Surname = customer.Surname;
                existingCustomer.Password = customer.Password;
                existingCustomer.SecurityGroup = customer.SecurityGroup;
            }
            return existingCustomer!;
        }
    }
}
