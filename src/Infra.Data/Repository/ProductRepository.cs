using Domain.Interfaces;
using Domain.Models;
using Infra.Data.Context;
using Microsoft.Extensions.Logging;

namespace Infra.Data.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(DataContext context
            //ILogger<IRepository<Product>> logger) : base(context, logger)
            ) : base(context)
        {
        }
    }
}
