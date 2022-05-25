using Microsoft.AspNetCore.Mvc;
using Shopping.Web.Helpers;

namespace Shopping.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly ICombosHelper _combosHelper;

        public AccountController(ICombosHelper combosHelper)
        {
            _combosHelper = combosHelper;
        }
    }
}
