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

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            return app;
        }

        private static bool IsAppInDebug()
        {
            #if DEBUG
            return true;
            #else
            return false;
            #endif

        }
    }
}
