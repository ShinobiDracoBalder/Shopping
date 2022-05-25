using Shopping.Web.Data.Entities;

namespace Shopping.Web.Interfaces
{
    public interface ICountryRepository : IFactoryRepository<Country>
    {
        public Task<Country> GetOnlyCountryAsync(int id);
        public Task<bool> GetAnyCountryAsync(int id);
        public Task<Country> GetAnyDetailsCountryAsync(int id);
    }
}

