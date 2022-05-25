using Microsoft.EntityFrameworkCore;
using Shopping.Web.Data;
using Shopping.Web.Data.Entities;
using Shopping.Web.Interfaces;

namespace Shopping.Web.Repositories
{
    public class StateRepository : FactoryRepository<State>, IStateRepository
    {
        private readonly DataContext _dataContext;

        public StateRepository(DataContext dataContext) : base(dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<State> GetAnyDetailsStateAsync(int id)
        {
            return await _dataContext.States
               .Include(s => s.Country)
               .Include(s => s.Cities)
               .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<bool> GetAnyStateAsync(int id)
        {
            return await _dataContext.States.AnyAsync(s => s.Id == id);
        }

        public async Task<State> GetOnlyStateAsync(int id)
        {
            return await _dataContext.States.FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
