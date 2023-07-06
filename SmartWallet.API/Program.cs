using SmartWallet.ApplicationService.Extension;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SmartWallet.API.JWT.Implementations;
using SmartWallet.API.JWT.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Generate a new JWT key and prepare authentication. (Generating a new key means users will log off if the server restarts)
var key = new byte[32];
using (var rng = RandomNumberGenerator.Create())
{
    rng.GetBytes(key);
}
var base64Key = Convert.ToBase64String(key);
var jwtSettings = new JwtProperties
{
    Key = base64Key,
    Issuer = "SmartWallet"
};

builder.Services.AddSingleton<IJwtProperties>(jwtSettings);


builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidIssuer = jwtSettings.Issuer,
    };
});


// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DBContext to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var authCredentials = builder.Configuration.GetConnectionString("BasicAuth");

// Custom DI Extension for linking application and infrastructure layers.
builder.Services.AddApplicationServices(connectionString);

var app = builder.Build();

// JWT
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
//#if DEBUG
app.UseSwagger();
app.UseSwaggerUI();
//#endif

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
