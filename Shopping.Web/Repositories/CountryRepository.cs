using Microsoft.EntityFrameworkCore;
using Shopping.Web.Data;
using Shopping.Web.Data.Entities;
using Shopping.Web.Interfaces;

namespace Shopping.Web.Repositories
{
    public class CountryRepository : FactoryRepository<Country>, ICountryRepository
    {
        private readonly DataContext _dataContext;

        public CountryRepository(DataContext dataContext) : base(dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> GetAnyCountryAsync(int id)
        {
           return  await  _dataContext.Countries.AnyAsync(e => e.Id == id);
        }

        public async Task<Country> GetOnlyCountryAsync(int id)
        {
           return await _dataContext.Countries.FirstOrDefaultAsync(c => c.Id.Equals(id));
        }
    }
}
