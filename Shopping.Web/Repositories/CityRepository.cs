using Microsoft.EntityFrameworkCore;
using Shopping.Web.Data;
using Shopping.Web.Data.Entities;
using Shopping.Web.Interfaces;

namespace Shopping.Web.Repositories
{
    public class CityRepository : FactoryRepository<City>, ICityRepository
    {
        private readonly DataContext _dataContext;

        public CityRepository(DataContext dataContext) : base(dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> GetAnyCityAsync(int id)
        {
            return await _dataContext.Cities.AnyAsync(c=>c.Id==id);
        }

        public async Task<City> GetOnlyCityAsync(int id)
        {
            return await _dataContext.Cities.FirstOrDefaultAsync(c => c.Id==id);
        }
    }
}
