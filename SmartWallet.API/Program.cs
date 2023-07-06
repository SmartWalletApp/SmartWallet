using SmartWallet.API.StartUpConfigurations;

var builder = new BuilderConfigurator(args).Builder;
var app = new AppConfigurator(builder).App;

app.Run();