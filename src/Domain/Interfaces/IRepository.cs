using Core.DomainObjects;
using System.Linq.Expressions;
using Core.Models;

namespace Domain.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity
    {
        Task Add(TEntity obj);
        Task Update(TEntity obj);
        Task Remove(Guid id);
        Task<TEntity?> GetById(Guid id);
        Task<IEnumerable<TEntity>> GetAll();
        Task<PagedResult<TEntity>> GetAll(int pageSize, int pageIndex, Expression<Func<TEntity, bool>>? expression = null);
        Task<IEnumerable<TEntity>> Search(Expression<Func<TEntity, bool>> expression, string[]? includes = null);
        Task<int> SaveChanges();
        IUnitOfWork UnitOfWork { get; }
    }
}
