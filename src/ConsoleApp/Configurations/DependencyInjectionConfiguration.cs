using Domain.Interfaces;
using Infra.Data.Context;
using Infra.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ConsoleApp.Configurations
{
    public static class DependencyInjectionConfiguration
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            var currentDirectory = string.Empty;
#if DEBUG
    currentDirectory = Directory.GetCurrentDirectory();
#else
    currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
#endif

            string environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var builder = new ConfigurationBuilder()
              .SetBasePath(currentDirectory)
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
              .AddJsonFile($"appsettings.{environmentName}.json", optional: true);

            IConfiguration configuration = builder.Build();

            //services.AddScoped(typeof(IMongoDbRepository<>), typeof(MongoDbRepository<>));
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IStateRepository, StateRepository>();
            services.AddTransient<IGovernorRepository, GovernorRepository>();

            var serviceProvider = new ServiceCollection()
                            .AddLogging(builder =>
                            {
                                builder.ClearProviders();
                                //builder.AddConsole();
                                //builder.AddDebug();
                            })
                            .BuildServiceProvider();

            ILogger<DataContext> logger = serviceProvider.GetService<ILogger<DataContext>>();            

            services.AddScoped<DataContext>(options =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<DataContext>();

                var provider = "SqlServer";

                if (provider == "SqlServer")
                {
                    var connectionString = configuration.GetConnectionString("SqlServer");
                    optionsBuilder
                        .UseSqlServer(connectionString)
                        .LogTo(Console.WriteLine, LogLevel.Information)
                        .EnableSensitiveDataLogging()
                        .EnableDetailedErrors();
                }

                if (provider == "MySql")
                {
                    var connectionString = configuration.GetConnectionString("MySql");
                    optionsBuilder
                        .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                        .LogTo(Console.WriteLine, LogLevel.Information)
                        .EnableSensitiveDataLogging()
                        .EnableDetailedErrors();
                }                

                return new DataContext(logger, optionsBuilder.Options);
                //return new DataContext(optionsBuilder.Options);
            });

            //services.AddDbContext<DataContext>(options =>
            //{
            //    //var provider = config.GetValue("provider", Sqlite.Name);
            //    var provider = "SqlServer";

            //    //if (provider == SqlServer.Name)
            //    if (provider == "SqlServer")
            //    {
            //        var connectionString = configuration.GetConnectionString("SqlServer");
            //        options
            //            .UseSqlServer(connectionString)
            //            .LogTo(Console.WriteLine, LogLevel.Information)
            //            .EnableSensitiveDataLogging()
            //            .EnableDetailedErrors();
            //    }

            //    //if (provider == MySql.Name)
            //    if (provider == "MySql")
            //    {
            //        var connectionString = configuration.GetConnectionString("MySql");
            //        options
            //            .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            //            .LogTo(Console.WriteLine, LogLevel.Information)
            //            .EnableSensitiveDataLogging()
            //            .EnableDetailedErrors();
            //    }
            //});
        }
    }
}
