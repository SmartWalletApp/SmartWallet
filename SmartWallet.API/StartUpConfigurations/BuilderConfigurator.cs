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

            var connectionString = Builder.Configuration.GetConnectionString("DefaultConnection");
            Builder.Configuration.GetConnectionString("BasicAuth");

            Builder.Services.AddApplicationServices(connectionString);

            return Builder;
        }
    }
}
