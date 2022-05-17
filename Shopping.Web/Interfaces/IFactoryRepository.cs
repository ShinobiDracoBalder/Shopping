namespace Shopping.Web.Interfaces
{
    public interface IFactoryRepository<TEntity> where TEntity : class
    {
        
        Task<ICollection<TEntity>> GetDatasAsync();
        Task<IEnumerable<TEntity>> Get();
        Task<TEntity> AddDataAsync(TEntity data);
        Task<bool> UpdateDataAsync(TEntity data);
        Task<bool> DeleteDataAsync(TEntity data);
        Task<bool> SaveAllAsync();
    }
}
