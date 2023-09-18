using Domain.Interfaces;
using Domain.Models;
using Infra.Data.Context;

namespace Infra.Data.Repository
{
    public class StateRepository : Repository<State>, IStateRepository
    {
        public StateRepository(DataContext context
            //ILogger<IRepository<State>> logger) : base(context, logger)
            ) : base(context)
        {
        }
    }
}
