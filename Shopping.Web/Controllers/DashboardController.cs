using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shopping.Web.Data;
using Shopping.Web.Interfaces;

namespace Shopping.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IUserHelperRepository _userHelperRepository;

        public DashboardController(DataContext dataContext, IUserHelperRepository userHelperRepository)
        {
            _dataContext = dataContext;
            _userHelperRepository = userHelperRepository;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
