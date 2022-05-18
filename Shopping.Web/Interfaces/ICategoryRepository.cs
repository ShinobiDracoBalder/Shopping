using Shopping.Web.Data.Entities;

namespace Shopping.Web.Interfaces
{
    public interface ICategoryRepository : IFactoryRepository<Category>
    {
        public Task<Category> GetOnlyCategoryAsync(int id);
        public Task<bool> GetAnyCategoryAsync(int id);
    }
}
