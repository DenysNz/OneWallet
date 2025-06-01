namespace Finance.Data.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity> Get(int id);

        IQueryable<TEntity> GetAll(bool track = false);

        Task Create(TEntity entity);

        Task Update(TEntity entity);

        Task Delete(int id);
    }
}
