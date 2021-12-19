using Microsoft.EntityFrameworkCore;
using NDDevicePoller;
using NDDevicePoller.Data;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext , services) =>
    {
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
