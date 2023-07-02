using ExchangeManager.ApplicationService.Contracts;
using ExchangeManager.ApplicationService.Services;
using ExchangeManager.DomainModel.RepositoryContracts;
using ExchangeManager.Infrastructure.DataModels;
using ExchangeManager.Infrastructure.Persistence;
using ExchangeManager.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeManager.ApplicationService.Extension
{
    public static class ExtensionBuilder
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IExchangeAppService, ExchangeAppService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IExchangeRepository<Coin>, ExchangeRepository<Coin>>();
            services.AddScoped<IExchangeRepository<Customer>, ExchangeRepository<Customer>>();
            services.AddScoped<IExchangeRepository<Wallet>, ExchangeRepository<Wallet>>();
            services.AddScoped<IExchangeRepository<BalanceHistory>, ExchangeRepository<BalanceHistory>>();
            return services;
        }
    }
}
