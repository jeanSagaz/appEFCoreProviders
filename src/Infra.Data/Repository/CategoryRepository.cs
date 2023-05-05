using Domain.Interfaces;
using Domain.Models;
using Infra.Data.Context;

namespace Infra.Data.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(DataContext context
            //ILogger<IRepository<Customer>> logger) : base(context, logger)
            ) : base(context)
        {
        }
    }
}
