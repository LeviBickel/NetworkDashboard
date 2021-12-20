using Microsoft.EntityFrameworkCore;
using NDDevicePoller;
using NDDevicePoller.Data;
using Serilog;

IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService()
    .UseSerilog()
    .ConfigureServices((hostContext , services) =>
    {
        Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(hostContext.Configuration).CreateLogger();
        IConfiguration configuration = hostContext.Configuration;

        AppSettings.Configuration = configuration;
        AppSettings.ConnectionString = configuration.GetConnectionString("DefaultConnection");

        var optionBuilder = new DbContextOptionsBuilder<DbContext>();
        optionBuilder.UseSqlServer(AppSettings.ConnectionString);
        services.AddScoped<DbContext>(d => new DbContext(optionBuilder.Options));
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
