using Domain.Interfaces;
using Domain.Models;
using Infra.Data.Context;

namespace Infra.Data.Repository
{
    public class GovernorRepository : Repository<Governor>, IGovernorRepository
    {
        public GovernorRepository(DataContext context
            //ILogger<IRepository<Governor>> logger) : base(context, logger)
            )
            : base(context)
        {
            
        }
    }
}
