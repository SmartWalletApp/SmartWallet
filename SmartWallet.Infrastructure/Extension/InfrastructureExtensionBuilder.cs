using SmartWallet.DomainModel.Persistence;
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
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string? connectionString)
        {
            services.AddDbContext<SmartWalletDbContext>(options =>
                options.UseMySql(connectionString, new MySqlServerVersion(new Version(10, 5, 19))));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ISmartWalletRepository<Coin>, SmartWalletRepository<Coin>>();
            services.AddScoped<ICustomerRepository<Customer>, CustomerRepository>();
            services.AddScoped<ISmartWalletRepository<BalanceHistory>, SmartWalletRepository<BalanceHistory>>();
            return services;
        }
    }
}
