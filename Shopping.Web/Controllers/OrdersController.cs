using Microsoft.AspNetCore.Mvc;
using Shopping.Web.Data;
using Vereyon.Web;

namespace Shopping.Web.Controllers
{
    public class OrdersController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IFlashMessage _flashMessage;

        public OrdersController(DataContext dataContext, IFlashMessage flashMessage)
        {
            _dataContext = dataContext;
            _flashMessage = flashMessage;
        }
    }
}
