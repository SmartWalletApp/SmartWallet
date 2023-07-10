using SmartWallet.API.StartUpConfigurations;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = new BuilderConfigurator(args).Builder;
        var app = new AppConfigurator(builder).App;

        app.Run();
    }
}