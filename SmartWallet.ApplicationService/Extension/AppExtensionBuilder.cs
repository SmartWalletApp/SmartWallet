using Microsoft.AspNetCore.Authentication.JwtBearer;
using SmartWallet.ApplicationService.Contracts;
using SmartWallet.ApplicationService.Services;
using Microsoft.Extensions.DependencyInjection;
using SmartWallet.Infrastructure.Extension;
using SmartWallet.ApplicationService.JWT.Contracts;
using SmartWallet.API.StartUpConfigurations;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FluentValidation;
using SmartWallet.ApplicationService.Validators;
using FluentValidation.AspNetCore;
using SmartWallet.ApplicationService.Dto.Request;

namespace SmartWallet.ApplicationService.Extension
{
    public static class AppExtensionBuilder
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, string connectionString)
        {
            var jwtProperties = JWTExtensionConfigurator.SetJWTProperties();

            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

            services.AddFluentValidationAutoValidation();
            services.AddScoped<IValidator<CustomerRequestDto>, CustomerValidator>();
            services.AddScoped<IValidator<CoinRequestDto>, CoinValidator>();
            services.AddScoped<IValidator<BalanceHistoryRequestDto>, BalanceHistoryValidator>();

            services.AddSingleton<IJwtProperties>(jwtProperties);
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtProperties.Key)),
                    ValidateIssuer = true,
                    ValidAlgorithms = new List<string> { SecurityAlgorithms.HmacSha512 },
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidIssuer = jwtProperties.Issuer,
                };
            });
            services.AddScoped<ISmartWalletAppService, SmartWalletAppService>();
            services.AddInfrastructureServices(connectionString);
            return services;
        }
    }
}
