using Microsoft.EntityFrameworkCore;
using Shopping.Web.Data;
using Shopping.Web.Data.Entities;
using Shopping.Web.Interfaces;

namespace Shopping.Web.Repositories
{
    public class CategoryRepository : FactoryRepository<Category>, ICategoryRepository
    {
        private readonly DataContext _dataContext;

        public CategoryRepository(DataContext dataContext) : base(dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> GetAnyCategoryAsync(int id)
        {
            return await _dataContext.Categories.AnyAsync(e => e.Id == id); ;
        }

        public async Task<Category> GetOnlyCategoryAsync(int id)
        {
            return await _dataContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
