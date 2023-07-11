using SmartWallet.DomainModel.RepositoryContracts;
using Microsoft.Extensions.DependencyInjection;
using SmartWallet.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using SmartWallet.Infrastructure.DataModels;
using SmartWallet.Infrastructure.RepositoryImplementations;

namespace SmartWallet.Infrastructure.Extension
{
    public static class InfrastructureExtensionBuilder
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString)
        {
            #if DEBUG
            services.AddDbContext<SmartWalletDbContext>(options =>
                options.UseSqlServer(connectionString));
            #else
            services.AddDbContext<SmartWalletDbContext>(options =>
                options.UseMySql(connectionString, new MySqlServerVersion(new Version(10, 5, 19))));
            #endif

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ISmartWalletRepository<Coin>, SmartWalletRepository<Coin>>();
            services.AddScoped<ISmartWalletRepository<BalanceHistoric>, SmartWalletRepository<BalanceHistoric>>();
            services.AddScoped<ICustomerRepository<Customer>, CustomerRepository>();
            services.AddScoped<ICoinRepository<Coin>, CoinRepository>();
            services.AddScoped<IWalletRepository<Wallet>, WalletRepository>();
            services.AddScoped<IBalanceHistoric<BalanceHistoric>, BalanceHistoricRepository>();
            return services;
        }
    }
}
