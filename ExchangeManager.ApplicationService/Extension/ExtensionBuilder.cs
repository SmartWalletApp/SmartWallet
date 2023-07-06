using ExchangeManager.ApplicationService.Contracts;
using ExchangeManager.ApplicationService.Services;
using ExchangeManager.DomainModel.Persistence;
using ExchangeManager.DomainModel.RepositoryContracts;
using ExchangeManager.Infrastructure.DataModels;
using ExchangeManager.Infrastructure.Persistence;
using ExchangeManager.Infrastructure.Repositories;
using ExchangeManager.Infrastructure.RepositoryImplementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeManager.ApplicationService.Extension
{
    public static class ExtensionBuilder
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, string? connectionString)
        {
            services.AddDbContext<ExchangeManagerDbContext>(options =>
                options.UseMySql(connectionString, new MySqlServerVersion(new Version(10, 5, 19))));

            services.AddScoped<IExchangeAppService, ExchangeAppService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IExchangeRepository<Coin>, ExchangeRepository<Coin>>();
            services.AddScoped<ICustomerRepository<Customer>, CustomerRepository>();
            services.AddScoped<IWalletRepository<Wallet>, WalletRepository>();
            services.AddScoped<IExchangeRepository<BalanceHistory>, ExchangeRepository<BalanceHistory>>();
            return services;
        }
    }
}
