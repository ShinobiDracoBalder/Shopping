using Microsoft.AspNetCore.Mvc;
using Shopping.Web.Interfaces;
<<<<<<< Updated upstream
=======
using Shopping.Web.Models;
using static Shopping.Web.Helpers.ModalHelper;
>>>>>>> Stashed changes

namespace Shopping.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserHelperRepository _userHelperRepository;

        public UsersController(IUserHelperRepository userHelperRepository)
        {
            _userHelperRepository = userHelperRepository;
<<<<<<< Updated upstream
=======
            _combosHelper = combosHelper;
            _dataContext = dataContext;
            _blobHelper = blobHelper;
            _mailHelper = mailHelper;
            _imageHelper = imageHelper;
        }
        public async Task<IActionResult> Index()
        {
           return View(await _userHelperRepository.GetUserAllAsync());   
        }

        [HttpGet]
        public async Task<IActionResult> DetailRegister(string id)
        {
            if (User.Identity.Name == null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }

            var _users = await _userHelperRepository.GetUserAsync(User.Identity.Name);

            if (_users == null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }

            if (id == null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }
            Guid userId = new Guid(id);
            var response = await _userHelperRepository.GetUserAsync(userId);
            if (response==null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }


            var model = new EditUserViewModel
            {
                Id = response.Id,
                FirstName = response.FirstName,
                LastName = response.LastName,
                Address = response.Address,
                PhoneNumber = response.PhoneNumber,
                Document = response.Document,
                PicturePath = response.PicturePath,
            };

            return View(model);
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
                string path = string.Empty;

                if (model.ImageFile != null)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                    path = await _imageHelper.UploadImageAsync(model.ImageFile, "users");
                }

                model.ImageId = imageId;
                model.PicturePath = path;
                User user = await _userHelperRepository.AddUserAsync(model);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Este correo ya está siendo usado.");
                    model.Countries = await _combosHelper.GetComboCountriesAsync();
                    model.States = await _combosHelper.GetComboStatesAsync(model.CountryId);
                    model.Cities = await _combosHelper.GetComboCitiesAsync(model.StateId);
                    return View(model);
                }

                string myToken = await _userHelperRepository.GenerateEmailConfirmationTokenAsync(user);
                string tokenLink = Url.Action("ConfirmEmail", "Account", new
                {
                    userid = user.Id,
                    token = myToken
                }, protocol: HttpContext.Request.Scheme);

                Response response = _mailHelper.SendMail(
                    $"{model.FirstName} {model.LastName}",
                    model.Username,
                    "Shopping - Confirmación de Email",
                    $"<h1>Shopping - Confirmación de Email</h1>" +
                        $"Para habilitar el usuario por favor hacer click en el siguiente link:, " +
                        $"<hr/><br/><p><a href = \"{tokenLink}\">Confirmar Email</a></p>");
                if (response.IsSuccess)
                {
                    ViewBag.Message = "Las instrucciones para habilitar el administrador han sido enviadas al correo.";
                    return View(model);
                }
                ModelState.AddModelError(string.Empty, response.Message);
            }

            model.Countries = await _combosHelper.GetComboCountriesAsync();
            model.States = await _combosHelper.GetComboStatesAsync(model.CountryId);
            model.Cities = await _combosHelper.GetComboCitiesAsync(model.StateId);
            return View(model);
        }

        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(string userId)
        {
            AddUserViewModel model = new();
            if (string.IsNullOrEmpty(userId))
            {
                {
                    model.Id = Guid.Empty.ToString();
                    model.Countries = await _combosHelper.GetComboCountriesAsync();
                    model.States = await _combosHelper.GetComboStatesAsync(0);
                    model.Cities = await _combosHelper.GetComboCitiesAsync(0);
                    model.UserType = UserType.Admin;
                };
                return View(model);
            }
            else
            {
                Guid _userId = new Guid(userId);
                User user = await _userHelperRepository.GetUserAsync(_userId);
                if (user == null)
                {
                    return NotFound();
                }
                model.Id = user.Id;
                model.Document = user.Document;
                model.PhoneNumber = user.PhoneNumber;
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.Address = user.Address;
                model.ImageId = user.ImageId;
                model.PicturePath = user.PicturePath;
                model.CityId = user.City.Id;
                model.StateId = user.City.State.Id;
                model.Countries = await _combosHelper.GetComboCountriesAsync();
                model.States = await _combosHelper.GetComboStatesAsync(user.City.State.Id);
                model.Cities = await _combosHelper.GetComboCitiesAsync(user.City.Id);
                model.UserType = UserType.Admin;
                return View(model);
            }
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
>>>>>>> Stashed changes
        }
    }
}
