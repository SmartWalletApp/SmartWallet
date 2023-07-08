using SmartWallet.DomainModel.Persistence;
using SmartWallet.Infrastructure.DataModels;
using SmartWallet.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace SmartWallet.Infrastructure.RepositoryImplementations
{
    public class CustomerRepository : SmartWalletRepository<Customer>, ICustomerRepository<Customer>
    {
        public CustomerRepository(SmartWalletDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<Customer> GetByEmail(string email)
        {
            return await EntitySet.FirstAsync(x => x.Email == email);
        }
        public override async Task<IEnumerable<Customer>> GetAll() =>
            await EntitySet.Include(c => c.Wallets)
                .ThenInclude(w => w.Coin)
                .Include(c => c.Wallets)
                .ThenInclude(w => w.BalanceHistory)
                .ToListAsync();

        public override async Task<Customer> GetByID(int Id) =>
            await EntitySet.Include(c => c.Wallets)
                .ThenInclude(w => w.Coin)
                .Include(c => c.Wallets)
                .ThenInclude(w => w.BalanceHistory)
                .FirstOrDefaultAsync(c => c.Id == Id);

        public override Task<Customer> Update(Customer customer)
        {
            var existingCustomer = EntitySet.FirstOrDefault(c => c.Email == customer.Email);

            if(existingCustomer != null)
            {
                existingCustomer.Name = customer.Name;
                existingCustomer.Surname = customer.Surname;
                existingCustomer.Password = customer.Password;
                existingCustomer.SecurityGroup = customer.SecurityGroup;
            }
            return Task.FromResult(existingCustomer);
        }
    }
}
