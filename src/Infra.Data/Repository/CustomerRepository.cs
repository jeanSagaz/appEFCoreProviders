using Domain.Interfaces;
using Domain.Models;
using Infra.Data.Context;
using Microsoft.Extensions.Logging;

namespace Infra.Data.Repository
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(DataContext context
            //ILogger<IRepository<Customer>> logger) : base(context, logger)
            ) : base(context)
        {
        }
    }
}
