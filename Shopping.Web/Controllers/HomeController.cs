using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Web.Data;
using Shopping.Web.Data.Entities;
using Shopping.Web.Interfaces;
using Shopping.Web.Models;
using System.Diagnostics;

namespace Shopping.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _dataContext;
        private readonly IUserHelperRepository _userHelperRepository;

        public HomeController(ILogger<HomeController> logger, DataContext dataContext, IUserHelperRepository userHelperRepository)
        {
            _logger = logger;
            _dataContext = dataContext;
            _userHelperRepository = userHelperRepository;
        }

        public async Task<IActionResult> Index()
        {
            List<Product>? products = await _dataContext.Products
            .Include(p => p.ProductImages)
            .Include(p => p.ProductCategories)
            .OrderBy(p => p.Description)
            .ToListAsync();
            List<ProductsHomeViewModel> productsHome = new() { new ProductsHomeViewModel() };
            int i = 1;
            foreach (Product? product in products)
            {
                if (i == 1)
                {
                    productsHome.LastOrDefault().Product1 = product;
                }
                if (i == 2)
                {
                    productsHome.LastOrDefault().Product2 = product;
                }
                if (i == 3)
                {
                    productsHome.LastOrDefault().Product3 = product;
                }
                if (i == 4)
                {
                    productsHome.LastOrDefault().Product4 = product;
                    productsHome.Add(new ProductsHomeViewModel());
                    i = 0;
                }
                i++;
            }
            HomeViewModel model = new() { Products = productsHome };
            User user = await _userHelperRepository.GetUserAsync(User.Identity.Name);
            if (user != null)
            {
                model.Quantity = await _dataContext.TemporalSales
                .Where(ts => ts.User.Id == user.Id)
                .SumAsync(ts => ts.Quantity);
            }
            return View(model);
        }

        public async Task<IActionResult> Add(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            Product product = await _dataContext.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            User user = await _userHelperRepository.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }
            TemporalSale temporalSale = new()
            {
                Product = product,
                Quantity = 1,
                User = user
            };
            _dataContext.TemporalSales.Add(temporalSale);
            await _dataContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("error/404")]
        public IActionResult Error404()
        {
            return View();
        }
    }
}