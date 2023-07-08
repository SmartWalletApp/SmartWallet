using ExchangeManager.ApplicationService.Extension;

namespace ExchangeManagerAPI.Configurations
{
    public class BuilderConfigurator
    {
        public static WebApplicationBuilder SetBuilder(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            AddBuilderServices(builder);

            return builder;
        }

        private static void AddBuilderServices(WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddApplicationServices(connectionString);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddApplicationServices(builder.Configuration.GetConnectionString("DefaultConnection"));


        }
    }
}
