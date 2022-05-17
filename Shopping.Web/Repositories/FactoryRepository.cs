using Microsoft.EntityFrameworkCore;
using Shopping.Web.Data;
using Shopping.Web.Interfaces;

namespace Shopping.Web.Repositories
{
    public class FactoryRepository<TEntity> : IFactoryRepository<TEntity> where TEntity : class
    {
        private readonly DataContext _dataContext;
       
        public FactoryRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
           
        }

        public async Task<TEntity> AddDataAsync(TEntity data)
        {
            await _dataContext.Set<TEntity>().AddAsync(data);
            await SaveAllAsync();
            return data;
        }

        public async Task<bool> DeleteDataAsync(TEntity data)
        {
            _dataContext.Set<TEntity>().Remove(data);
            return await SaveAllAsync();
        }

        public async Task<IEnumerable<TEntity>> Get()
        {
            return await _dataContext.Set<TEntity>().ToListAsync();
        }

        public async Task<ICollection<TEntity>> GetDatasAsync()
        {
          return await _dataContext.Set<TEntity>().ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateDataAsync(TEntity data)
        {
            _dataContext.Set<TEntity>().Update(data);
            return await SaveAllAsync();
        }
    }
}
