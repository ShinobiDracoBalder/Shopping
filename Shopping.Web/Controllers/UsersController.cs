using Microsoft.AspNetCore.Mvc;
using Shopping.Web.Interfaces;

namespace Shopping.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserHelperRepository _userHelperRepository;

        public UsersController(IUserHelperRepository userHelperRepository)
        {
            _userHelperRepository = userHelperRepository;
        }
    }
}
