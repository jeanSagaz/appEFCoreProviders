using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Core.Providers.DatabaseFlavor;
using static Core.Providers.DatabaseFlavor.ProviderConfiguration;
using static Core.Providers.DatabaseFlavor.ProviderSelector;

namespace WebApp.Extensions
{
    public static class ConfigureEFCoreExtension
    {
        public static void AddEFCoreConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            //services.AddScoped<DataContext>(provider =>
            //{
            //    ILogger<DataContext> logger = services.BuildServiceProvider().GetService<ILogger<DataContext>>();

            //    var optionsBuilder = new DbContextOptionsBuilder<DataContext>();

            //    //var connectionString = configuration.GetConnectionString("SqlServer");
            //    //optionsBuilder
            //    //    .UseSqlServer(connectionString)
            //    //    .LogTo(Console.WriteLine, LogLevel.Information)
            //    //    .EnableSensitiveDataLogging()
            //    //    .EnableDetailedErrors();

            //    var connectionString = configuration.GetConnectionString("MySql");
            //    optionsBuilder
            //        .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            //        .LogTo(Console.WriteLine, LogLevel.Information)
            //        .EnableSensitiveDataLogging()
            //        .EnableDetailedErrors();

            //    return new DataContext(logger, optionsBuilder.Options);
            //});

            services.AddDbContext<DataContext>(options =>
            {
                //var provider = configuration.GetValue("provider", Sqlite.Name);
                var provider = "SqlServer";

                switch (provider)
                {
                    case "SqlServer":

                        var connectionString = configuration.GetConnectionString("DefaultConnection");
                        options
                            .UseSqlServer(connectionString)
                            .LogTo(Console.WriteLine, LogLevel.Information)
                            .EnableSensitiveDataLogging()
                            .EnableDetailedErrors();

                        break;

                    case "MySql":

                        connectionString = configuration.GetConnectionString("MySql");
                        options
                            .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), p => p.SchemaBehavior(MySqlSchemaBehavior.Ignore))
                            .LogTo(Console.WriteLine, LogLevel.Information)
                            .EnableSensitiveDataLogging()
                            .EnableDetailedErrors();

                        break;
                }
            });
        }

        public static void AddEFCoreApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureProviderForContext<DataContext>(DetectDatabase(configuration));

            //services.AddDbContext<DataContext>(WithProviderAutoSelection(DetectDatabase(configuration)));
                
        }
    }
}
