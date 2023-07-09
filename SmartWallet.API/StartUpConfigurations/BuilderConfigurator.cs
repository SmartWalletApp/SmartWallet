using Microsoft.AspNetCore.Server.Kestrel.Core;
using SmartWallet.ApplicationService.Extension;

namespace SmartWallet.API.StartUpConfigurations
{
    public class BuilderConfigurator
    {
        public WebApplicationBuilder Builder;

        public BuilderConfigurator(string[] args)
        {
            Builder = SetBuilder(args);
        }

        private WebApplicationBuilder SetBuilder(string[] args)
        {
            Builder = WebApplication.CreateBuilder(args);

            Builder.Services.AddControllers();
            Builder.Services.AddEndpointsApiExplorer();
            Builder.Services.AddSwaggerGen();

            // Prevent Kestrel from adding a Server header
            Builder.Services.Configure<KestrelServerOptions>(options =>
            {
                options.AddServerHeader = false;
            });

            #if DEBUG
            var connectionString = Builder.Configuration.GetConnectionString("MSSQL");
            #else
            var connectionString = Builder.Configuration.GetConnectionString("MySQL");
            #endif

            Builder.Services.AddApplicationServices(connectionString);

            return Builder;
        }
    }
}
