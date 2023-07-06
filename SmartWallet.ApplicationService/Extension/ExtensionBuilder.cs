using SmartWallet.ApplicationService.Contracts;
using SmartWallet.ApplicationService.Services;
using SmartWallet.DomainModel.Persistence;
using SmartWallet.DomainModel.RepositoryContracts;
using SmartWallet.Infrastructure.DataModels;
using SmartWallet.Infrastructure.Persistence;
using SmartWallet.Infrastructure.Repositories;
using SmartWallet.Infrastructure.RepositoryImplementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SmartWallet.ApplicationService.Extension
{
    public static class ExtensionBuilder
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, string? connectionString)
        {
            services.AddDbContext<SmartWalletDbContext>(options =>
                options.UseMySql(connectionString, new MySqlServerVersion(new Version(10, 5, 19))));

            /*
            MICROSOFT SQL
                builder.Services.AddDbContext<SmartWalletDbContext>(options =>
                options.UseSqlServer(connectionString));
            */

            services.AddScoped<ISmartWalletAppService, SmartWalletAppService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ISmartWalletRepository<Coin>, SmartWalletRepository<Coin>>();
            services.AddScoped<ICustomerRepository<Customer>, CustomerRepository>();
            services.AddScoped<ISmartWalletRepository<BalanceHistory>, SmartWalletRepository<BalanceHistory>>();
            return services;
        }
    }
}
