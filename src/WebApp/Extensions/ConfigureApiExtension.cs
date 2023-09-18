using Domain.Interfaces;
using Infra.Data.Repository;

namespace WebApp.Extensions
{
    public static class ConfigureApiExtension
    {
        public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IStateRepository, StateRepository>();
            services.AddTransient<IGovernorRepository, GovernorRepository>();
        }
    }
}
