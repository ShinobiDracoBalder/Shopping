using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shooping.Common.Enums;
using Shopping.Web.Data;
using Shopping.Web.Data.Entities;
using Shopping.Web.Helpers;
using Shopping.Web.Interfaces;
using Shopping.Web.Models;

namespace Shopping.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserHelperRepository _userHelperRepository;
        private readonly ICombosHelper _combosHelper;
        private readonly DataContext _dataContext;

        public UsersController(IUserHelperRepository userHelperRepository
            , ICombosHelper combosHelper,DataContext dataContext)
        {
            _userHelperRepository = userHelperRepository;
            _combosHelper = combosHelper;
            _dataContext = dataContext;
        }
        public async Task<IActionResult> Index()
        {
           return View(await _userHelperRepository.GetUserAllAsync());   
        }
        public async Task<IActionResult> Create()
        {
            AddUserViewModel model = new(){
                Id = Guid.Empty.ToString(),
                Countries = await _combosHelper.GetComboCountriesAsync(),
                States = await _combosHelper.GetComboStatesAsync(0),
                Cities = await _combosHelper.GetComboCitiesAsync(0),
                UserType = UserType.Admin,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (model.ImageFile != null)
                {
                    //imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                }

                model.ImageId = imageId;
                User user = await _userHelperRepository.AddUserAsync(model);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Este correo ya está siendo usado.");
                    model.Countries = await _combosHelper.GetComboCountriesAsync();
                    model.States = await _combosHelper.GetComboStatesAsync(model.CountryId);
                    model.Cities = await _combosHelper.GetComboCitiesAsync(model.StateId);
                    return View(model);
                }

                return RedirectToAction("Index", "Home");
            }

            model.Countries = await _combosHelper.GetComboCountriesAsync();
            model.States = await _combosHelper.GetComboStatesAsync(model.CountryId);
            model.Cities = await _combosHelper.GetComboCitiesAsync(model.StateId);
            return View(model);
        }

        public JsonResult GetStates(int countryId)
        {
            Country country = _dataContext.Countries
                .Include(c => c.States)
                .FirstOrDefault(c => c.Id == countryId);
            if (country == null)
            {
                return null;
            }

            return Json(country.States.OrderBy(d => d.Name));
        }

        public JsonResult GetCities(int stateId)
        {
            State state = _dataContext.States
                .Include(s => s.Cities)
                .FirstOrDefault(s => s.Id == stateId);
            if (state == null)
            {
                return null;
            }

            return Json(state.Cities.OrderBy(c => c.Name));
        }
    }
}
