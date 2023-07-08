namespace ExchangeManagerAPI.Configurations
{
    public class AppConfigurator
    {
        public static WebApplication BuildApp(WebApplicationBuilder builder)
        {
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (IsAppInDebug()) {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            return app;
        }

        private static bool IsAppInDebug() {
            #if DEBUG
            return true;
            #endif
        }
    }
}