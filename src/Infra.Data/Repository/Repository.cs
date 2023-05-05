using Core.DomainObjects;
using Core.Models;
using Domain.Interfaces;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Infra.Data.Repository
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        protected readonly DataContext _context;
        protected readonly DbSet<TEntity> _dbSet;
        private readonly ILogger<IRepository<TEntity>> _logger;

        protected Repository(DataContext context) 
            //ILogger<IRepository<TEntity>> logger)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
            //_logger = logger;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async virtual Task Add(TEntity obj)
        {
            try
            {
                await _dbSet.AddAsync(obj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao salvar o objeto no banco de dados.");
                throw;
            }
        }

        public async virtual Task Update(TEntity obj)
        {
            try
            {
                _dbSet.Update(obj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao atualizar o objeto no banco de dados.");
                throw;
            }
        }

        public async virtual Task Remove(Guid id)
        {
            try
            {
                _dbSet.Remove(await GetById(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao apagar o registro no banco de dados.");
                throw;
            }
        }

        public async virtual Task<IEnumerable<TEntity>> GetAll()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }        

        public async virtual Task<PagedResult<TEntity>> GetAll(int pageSize, int pageIndex, Expression<Func<TEntity, bool>>? expression = null)
        {
            var count = 0;
            var list = new List<TEntity>();

            if (expression is not null)
            {
                count = _dbSet.AsNoTracking().Where(expression).Count();
                list = await _dbSet.AsNoTracking().Skip(pageSize * (pageIndex - 1)).Take(pageSize).OrderByDescending(x => x.DateCreated).Where(expression).ToListAsync();
            }
            else
            {
                count = _dbSet.AsNoTracking().Count();
                list = await _dbSet.AsNoTracking().Skip(pageSize * (pageIndex - 1)).Take(pageSize).OrderByDescending(x => x.DateCreated).ToListAsync();
            }

            return new PagedResult<TEntity>()
            {
                List = list,
                TotalResults = count,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Query = expression?.Name ?? string.Empty
            };
        }

        public async virtual Task<TEntity?> GetById(Guid id)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async virtual Task<IEnumerable<TEntity>> Search(Expression<Func<TEntity, bool>> expression, string[]? includes = null)
        {
            var query = _dbSet.AsNoTracking().AsQueryable();

            if (includes is not null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.Where(expression).ToListAsync();
        }

        public async virtual Task<int> SaveChanges()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro no banco de dados.");
                throw;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }        
    }
}
