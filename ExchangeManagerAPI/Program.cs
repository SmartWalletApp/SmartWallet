using ExchangeManager.ApplicationService.Extension;
using ExchangeManager.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Custom DI Extension for linking application and infrastructure layers.
builder.Services.AddApplicationServices();

// Add DBContext to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
/* MYSQL */
builder.Services.AddDbContext<ExchangeManagerDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(10, 5, 19))));

/*
MICROSOFT SQL
    builder.Services.AddDbContext<ExchangeManagerDbContext>(options =>
    options.UseSqlServer(connectionString));
*/

var app = builder.Build();

// Configure the HTTP request pipeline.
#if DEBUG
app.UseSwagger();
app.UseSwaggerUI();
#endif

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
