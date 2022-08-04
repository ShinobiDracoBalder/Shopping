using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shooping.Common.Enums;
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
            ViewBag.UsersCount = _dataContext.Users.Count();
            ViewBag.ProductsCount = _dataContext.Products.Count();
            ViewBag.NewOrdersCount = _dataContext.Sales.Where(o => o.OrderStatus == OrderStatus.Nuevo).Count();
            ViewBag.ConfirmedOrdersCount = _dataContext.Sales.Where(o => o.OrderStatus == OrderStatus.Confirmado).Count();

            return View(await _dataContext.TemporalSales
                    .Include(u => u.User)
                    .Include(p => p.Product).ToListAsync());
        }
    }
}
