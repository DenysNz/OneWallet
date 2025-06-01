using Microsoft.EntityFrameworkCore;
using Finance.Data.Repositories.Interfaces;

namespace Finance.Data.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly FinanceDbContext _dbContext;

        public GenericRepository(FinanceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TEntity> Get(int id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        public IQueryable<TEntity> GetAll(bool track = false)
        {
            IQueryable<TEntity> res = _dbContext.Set<TEntity>();

            if (!track)
            {
                res = res.AsNoTracking();
            }

            return res;
        }

        public async Task Create(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var entity = await _dbContext.Set<TEntity>().FindAsync(id);
            _dbContext.Set<TEntity>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
