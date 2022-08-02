using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Web.Data;
using Shopping.Web.Data.Entities;
using Shopping.Web.Interfaces;
using Vereyon.Web;

namespace Shopping.Web.Controllers
{
    public class OrdersController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IFlashMessage _flashMessage;
        private readonly IOrdersHelper _ordersHelper;

        public OrdersController(DataContext dataContext, IFlashMessage flashMessage, IOrdersHelper ordersHelper)
        {
            _dataContext = dataContext;
            _flashMessage = flashMessage;
            _ordersHelper = ordersHelper;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Sales
                .Include(s => s.User)
                .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Product)
                .ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Sale sale = await _dataContext.Sales
                .Include(s => s.User)
                .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Product)
                .ThenInclude(p => p.ProductImages)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (sale == null)
            {
                return NotFound();
            }

            return View(sale);
        }
    }
}
