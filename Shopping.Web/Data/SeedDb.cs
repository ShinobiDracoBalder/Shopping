using Shooping.Common.Enums;
using Shopping.Web.Data.Entities;
using Shopping.Web.Helpers;
using Shopping.Web.Interfaces;

namespace Shopping.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _dataContext;
        private readonly IUserHelperRepository _userHelperRepository;
        private readonly IImageHelper _imageHelper;

        public SeedDb(DataContext dataContext, IUserHelperRepository userHelperRepository, IImageHelper imageHelper)
        {
            _dataContext = dataContext;
            _userHelperRepository = userHelperRepository;
            _imageHelper = imageHelper;
        }
        public async Task SeedAsync()
        {
            await _dataContext.Database.EnsureCreatedAsync();
            await CheckCategoriesAsync();
            await CheckCountriesAsync();
            await CheckRolesAsync();
            await CheckUserAsync("1010", "Juan", "Zuluaga", "zulu@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", "JuanZuluaga.jpeg", UserType.Admin);
            await CheckUserAsync("911", "Draco", "Master", "miles58y_r554v@hxsni.com", "322 311 4620", "Fin del Mundo", "KakashiNaruto.jpg", UserType.Admin);
            await CheckUserAsync("2020", "Ledys", "Bedoya", "ledys@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", "LedysBedoya.jpeg", UserType.User);
            //await CheckUserAsync("3030", "Brad", "Pitt", "brad@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", "Brad.jpg", UserType.User);
            //await CheckUserAsync("4040", "Angelina", "Jolie", "angelina@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", "Angelina.jpg", UserType.User);
            //await CheckUserAsync("5050", "Bob", "Marley", "bob@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", "bob.jpg", UserType.User);
            //await CheckProductsAsync();
        }
        private async Task CheckCountriesAsync()
        {
            if (!_dataContext.Countries.Any())
            {
                _dataContext.Countries.Add(new Country
                {
                    Name = "Colombia",
                    States = new List<State>()
            {
                new State()
                {
                    Name = "Antioquia",
                    Cities = new List<City>() {
                        new City() { Name = "Medellín" },
                        new City() { Name = "Itagüí" },
                        new City() { Name = "Envigado" },
                        new City() { Name = "Bello" },
                        new City() { Name = "Sabaneta" },
                        new City() { Name = "La Ceja" },
                        new City() { Name = "La Union" },
                        new City() { Name = "La Estrella" },
                        new City() { Name = "Copacabana" },
                    }
                },
                new State()
                {
                    Name = "Bogotá",
                    Cities = new List<City>() {
                        new City() { Name = "Usaquen" },
                        new City() { Name = "Champinero" },
                        new City() { Name = "Santa fe" },
                        new City() { Name = "Usme" },
                        new City() { Name = "Bosa" },
                    }
                },
                new State()
                {
                    Name = "Valle",
                    Cities = new List<City>() {
                        new City() { Name = "Calí" },
                        new City() { Name = "Jumbo" },
                        new City() { Name = "Jamundí" },
                        new City() { Name = "Chipichape" },
                        new City() { Name = "Buenaventura" },
                        new City() { Name = "Cartago" },
                        new City() { Name = "Buga" },
                        new City() { Name = "Palmira" },
                    }
                },
                new State()
                {
                    Name = "Santander",
                    Cities = new List<City>() {
                        new City() { Name = "Bucaramanga" },
                        new City() { Name = "Málaga" },
                        new City() { Name = "Barrancabermeja" },
                        new City() { Name = "Rionegro" },
                        new City() { Name = "Barichara" },
                        new City() { Name = "Zapatoca" },
                    }
                },
            }
                });
                _dataContext.Countries.Add(new Country
                {
                    Name = "Estados Unidos",
                    States = new List<State>()
            {
                new State()
                {
                    Name = "Florida",
                    Cities = new List<City>() {
                        new City() { Name = "Orlando" },
                        new City() { Name = "Miami" },
                        new City() { Name = "Tampa" },
                        new City() { Name = "Fort Lauderdale" },
                        new City() { Name = "Key West" },
                    }
                },
                new State()
                {
                    Name = "Texas",
                    Cities = new List<City>() {
                        new City() { Name = "Houston" },
                        new City() { Name = "San Antonio" },
                        new City() { Name = "Dallas" },
                        new City() { Name = "Austin" },
                        new City() { Name = "El Paso" },
                    }
                },
                new State()
                {
                    Name = "California",
                    Cities = new List<City>() {
                        new City() { Name = "Los Angeles" },
                        new City() { Name = "San Francisco" },
                        new City() { Name = "San Diego" },
                        new City() { Name = "San Bruno" },
                        new City() { Name = "Sacramento" },
                        new City() { Name = "Fresno" },
                    }
                },
            }
                });
                _dataContext.Countries.Add(new Country
                {
                    Name = "Ecuador",
                    States = new List<State>()
            {
                new State()
                {
                    Name = "Pichincha",
                    Cities = new List<City>() {
                        new City() { Name = "Quito" },
                    }
                },
                new State()
                {
                    Name = "Esmeraldas",
                    Cities = new List<City>() {
                        new City() { Name = "Esmeraldas" },
                    }
                },
            }
                });
            }

            await _dataContext.SaveChangesAsync();
        }
        private async Task CheckCategoriesAsync()
        {
            if (!_dataContext.Categories.Any())
            {
                _dataContext.Categories.Add(new Category { Name = "Tecnología" });
                _dataContext.Categories.Add(new Category { Name = "Ropa" });
                _dataContext.Categories.Add(new Category { Name = "Gamer" });
                _dataContext.Categories.Add(new Category { Name = "Belleza" });
                _dataContext.Categories.Add(new Category { Name = "Nutrición" });
                _dataContext.Categories.Add(new Category { Name = "Calzado" });
                _dataContext.Categories.Add(new Category { Name = "Deportes" });
                _dataContext.Categories.Add(new Category { Name = "Mascotas" });
                _dataContext.Categories.Add(new Category { Name = "Apple" });
                await _dataContext.SaveChangesAsync();
            }
        }

        private async Task<User> CheckUserAsync(
            string document,
            string firstName,
            string lastName,
            string email,
            string phone,
            string address,
            string image,
            UserType userType)
        {
            User user = await _userHelperRepository.GetUserAsync(email);
            if (user == null)
            {
                string imageId = $"~/images/users/{image}";
                // Guid imageId = await _blobHelper.UploadBlobAsync($"{Environment.CurrentDirectory}\\wwwroot\\images\\users\\{image}", "users");
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    City = _dataContext.Cities.FirstOrDefault(),
                    UserType = userType,
                    PicturePath = imageId
                };

                await _userHelperRepository.AddUserAsync(user, "123456");
                await _userHelperRepository.AddUserToRoleAsync(user, userType.ToString());

                string token = await _userHelperRepository.GenerateEmailConfirmationTokenAsync(user);
                await _userHelperRepository.ConfirmEmailAsync(user, token);
            }

            return user;
        }

        private async Task CheckRolesAsync()
        {
            await _userHelperRepository.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelperRepository.CheckRoleAsync(UserType.User.ToString());
        }
    }
}
