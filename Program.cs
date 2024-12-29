using Microsoft.EntityFrameworkCore;
using reactCrudBackend.Config;
using reactCrudBackend.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "_myAllowSpecificorigins",
        policy =>
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        });
});

// Load the external dbconfig.properties file
var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddIniFile("dbconfig.properties", optional: true, reloadOnChange: true)
    .Build();

// Bind configuration to DbConfig class
var dbConfig = new DbConfig();
config.GetSection("DbConfig").Bind(dbConfig);

if (dbConfig?.DbServer == null || dbConfig?.DbPort == null)
{
    throw new InvalidOperationException("Database configuration is incomplete.");
}

// Register DbConfig to be used in the DI container
builder.Services.AddSingleton(dbConfig);

// Configure DbContext with the credentials from the property file
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        $"Server={dbConfig.DbServer};Port={dbConfig.DbPort};Database={dbConfig.DbName};User={dbConfig.DbUser};Password={dbConfig.DbPassword}",
        new MySqlServerVersion(new Version(8, 0, 21))));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors("_myAllowSpecificorigins");
app.MapControllers();

app.Run();
