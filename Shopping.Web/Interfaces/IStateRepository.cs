using Shopping.Web.Data.Entities;

namespace Shopping.Web.Interfaces
{
    public interface IStateRepository : IFactoryRepository<State>
    {
        public Task<State> GetOnlyStateAsync(int id);
        public Task<bool> GetAnyStateAsync(int id);
        public Task<State> GetAnyDetailsStateAsync(int id); 
    }
}
