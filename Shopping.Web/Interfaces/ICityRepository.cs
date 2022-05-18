using Shopping.Web.Data.Entities;

namespace Shopping.Web.Interfaces
{
    public interface ICityRepository : IFactoryRepository<City>
    {
        public Task<City> GetOnlyCityAsync(int id);
        public Task<bool> GetAnyCityAsync(int id);
    }
}
