using SmartWallet.ApplicationService.Contracts;
using SmartWallet.DomainModel.RepositoryContracts;

namespace SmartWallet.API.StartUpConfigurations
{
    public class AppConfigurator
    {
        public WebApplication App;

        public AppConfigurator(WebApplicationBuilder builder) {
            App = BuildApp(builder);
        }
        public static WebApplication BuildApp(WebApplicationBuilder builder)
        {
            var app = builder.Build();

            // Run database restoration if it doesn't exist
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var unitOfWork = services.GetRequiredService<IUnitOfWork>();
                unitOfWork.EnsureCreated();
                unitOfWork.Save();
            }

            app.UseSwagger();
            app.UseSwaggerUI();

            // app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            return app;
        }
    }
}
