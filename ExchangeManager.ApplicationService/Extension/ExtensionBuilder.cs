using ExchangeManager.ApplicationService.Contracts;
using ExchangeManager.ApplicationService.Services;
using ExchangeManager.DomainModel.RepositoryContracts;
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
            services.AddScoped<IExchangeRepository, ExchangeRepository>();
            return services;
        }
    }
}
