using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Web.Data;
using Shopping.Web.Data.Entities;
using Shopping.Web.Helpers;
using Shopping.Web.Models;
using Vereyon.Web;
using static Shopping.Web.Helpers.ModalHelper;

namespace Shopping.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly ICombosHelper _combosHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IFlashMessage _flashMessage;
        private readonly IImageHelper _imageHelper;

        public ProductsController(DataContext dataContext, ICombosHelper combosHelper,
            IBlobHelper blobHelper, IFlashMessage flashMessage, IImageHelper imageHelper)
        {
            _dataContext = dataContext;
            _combosHelper = combosHelper;
            _blobHelper = blobHelper;
            _flashMessage = flashMessage;
            _imageHelper = imageHelper;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
                .ToListAsync());
        }

        [NoDirectAccess]
        public async Task<IActionResult> Create()
        {
            CreateProductViewModel model = new()
            {
                Categories = await _combosHelper.GetComboCategoriesAsync(),
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;
                string path = string.Empty;
                if (model.ImageFile != null)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "products");
                    path = await _imageHelper.UploadImageAsync(model.ImageFile, "products");
                }

                Product product = new()
                {
                    Description = model.Description,
                    Name = model.Name,
                    Price = model.Price,
                    Stock = model.Stock,
                };

                product.ProductCategories = new List<ProductCategory>()
                {
                    new ProductCategory
                    {
                        Category = await _dataContext.Categories.FindAsync(model.CategoryId)
                    }
                };

                if (imageId != Guid.Empty)
                {
                    product.ProductImages = new List<ProductImage>()
                    {
                        new ProductImage { ImageId = imageId, ImagePath = path }
                    };
                }

                try
                {
                    _dataContext.Add(product);
                    await _dataContext.SaveChangesAsync();
                    _flashMessage.Confirmation("Registro creado.");
                    return Json(new
                    {
                        isValid = true,
                        html = ModalHelper.RenderRazorViewToString(this, "_ViewAllProducts", _dataContext.Products
                        .Include(p => p.ProductImages)
                        .Include(p => p.ProductCategories)
                        .ThenInclude(pc => pc.Category).ToList())
                    });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe un producto con el mismo nombre.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            model.Categories = await _combosHelper.GetComboCategoriesAsync();
            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "Create", model) });
        }

        [NoDirectAccess]
        public async Task<IActionResult> Edit(int id)
        {
            Product product = await _dataContext.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            EditProductViewModel model = new()
            {
                Description = product.Description,
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateProductViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            try
            {
                Product product = await _dataContext.Products.FindAsync(model.Id);
                product.Description = model.Description;
                product.Name = model.Name;
                product.Price = model.Price;
                product.Stock = model.Stock;
                _dataContext.Update(product);
                await _dataContext.SaveChangesAsync();
                _flashMessage.Confirmation("Registro actualizado.");
                return Json(new
                {
                    isValid = true,
                    html = ModalHelper.RenderRazorViewToString(this, "_ViewAllProducts", _dataContext.Products
                    .Include(p => p.ProductImages)
                    .Include(p => p.ProductCategories)
                    .ThenInclude(pc => pc.Category).ToList())
                });
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    ModelState.AddModelError(string.Empty, "Ya existe un producto con el mismo nombre.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }

            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "Edit", model) });
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product product = await _dataContext.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [NoDirectAccess]
        public async Task<IActionResult> AddImage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product product = await _dataContext.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            AddProductImageViewModel model = new()
            {
                ProductId = product.Id,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddImage(AddProductImageViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "products");
                string ImagePath = await _imageHelper.UploadImageAsync(model.ImageFile, "products");
                Product product = await _dataContext.Products.FindAsync(model.ProductId);
                ProductImage productImage = new()
                {
                    Product = product,
                    ImageId = imageId,
                    ImagePath = ImagePath
                };

                try
                {
                    _dataContext.Add(productImage);
                    await _dataContext.SaveChangesAsync();
                    _flashMessage.Confirmation("Imagen agregada.");
                    return Json(new
                    {
                        isValid = true,
                        html = ModalHelper.RenderRazorViewToString(this, "Details", _dataContext.Products
                            .Include(p => p.ProductImages)
                            .Include(p => p.ProductCategories)
                            .ThenInclude(pc => pc.Category)
                            .FirstOrDefaultAsync(p => p.Id == model.ProductId))
                    });
                }
                catch (Exception exception)
                {
                    _flashMessage.Danger(exception.Message);
                }
            }

            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "AddImage", model) });
        }

        public async Task<IActionResult> DeleteImage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ProductImage productImage = await _dataContext.ProductImages
                .Include(pi => pi.Product)
                .FirstOrDefaultAsync(pi => pi.Id == id);
            if (productImage == null)
            {
                return NotFound();
            }

            await _blobHelper.DeleteBlobAsync(productImage.ImageId, "products");
            _dataContext.ProductImages.Remove(productImage);
            await _dataContext.SaveChangesAsync();
            _flashMessage.Info("Registro borrado.");
            return RedirectToAction(nameof(Details), new { id = productImage.Product.Id });
        }

        [NoDirectAccess]
        public async Task<IActionResult> AddCategory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product product = await _dataContext.Products
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            List<Category> categories = product.ProductCategories.Select(pc => new Category
            {
                Id = pc.Category.Id,
                Name = pc.Category.Name,
            }).ToList();

            AddCategoryProductViewModel model = new()
            {
                ProductId = product.Id,
                Categories = await _combosHelper.GetComboCategoriesAsync(categories),
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCategory(AddCategoryProductViewModel model)
        {
            Product product = await _dataContext.Products
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
                .FirstOrDefaultAsync(p => p.Id == model.ProductId);

            if (ModelState.IsValid)
            {
                ProductCategory productCategory = new()
                {
                    Category = await _dataContext.Categories.FindAsync(model.CategoryId),
                    Product = product,
                };

                try
                {
                    _dataContext.Add(productCategory);
                    await _dataContext.SaveChangesAsync();
                    _flashMessage.Confirmation("Categoría agregada.");
                    return Json(new
                    {
                        isValid = true,
                        html = ModalHelper.RenderRazorViewToString(this, "Details", _dataContext.Products
                            .Include(p => p.ProductImages)
                            .Include(p => p.ProductCategories)
                            .ThenInclude(pc => pc.Category)
                            .FirstOrDefaultAsync(p => p.Id == model.ProductId))
                    });
                }
                catch (Exception exception)
                {
                    _flashMessage.Danger(exception.Message);
                }
            }

            List<Category> categories = product.ProductCategories.Select(pc => new Category
            {
                Id = pc.Category.Id,
                Name = pc.Category.Name,
            }).ToList();

            model.Categories = await _combosHelper.GetComboCategoriesAsync(categories);
            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "AddCategory", model) });
        }

        public async Task<IActionResult> DeleteCategory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ProductCategory productCategory = await _dataContext.ProductCategories
                .Include(pc => pc.Product)
                .FirstOrDefaultAsync(pc => pc.Id == id);
            if (productCategory == null)
            {
                return NotFound();
            }

            _dataContext.ProductCategories.Remove(productCategory);
            await _dataContext.SaveChangesAsync();
            _flashMessage.Info("Registro borrado.");
            return RedirectToAction(nameof(Details), new { Id = productCategory.Product.Id });
        }


        [NoDirectAccess]
        public async Task<IActionResult> Delete(int id)
        {
            Product product = await _dataContext.Products
                .Include(p => p.ProductCategories)
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            foreach (ProductImage productImage in product.ProductImages)
            {
                await _blobHelper.DeleteBlobAsync(productImage.ImageId, "products");
            }

            _dataContext.Products.Remove(product);
            await _dataContext.SaveChangesAsync();
            _flashMessage.Info("Registro borrado.");
            return RedirectToAction(nameof(Index));
        }
        
    }
}
