using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Web.Data;
using Shopping.Web.Data.Entities;
using Shopping.Web.Helpers;
using Shopping.Web.Interfaces;
using Vereyon.Web;
using static Shopping.Web.Helpers.ModalHelper;

namespace Shopping.Web.Controllers
{
    public class CategoriesController: Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly DataContext _dataContext;
        private readonly IFlashMessage _flashMessage;

        public CategoriesController(ICategoryRepository categoryRepository, DataContext dataContext, IFlashMessage flashMessage)
        {
            _categoryRepository = categoryRepository;
            _dataContext = dataContext;
            _flashMessage = flashMessage;
        }
        // GET: Countries
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Categories
                .Include(c => c.ProductCategories)
                .ToListAsync());
        }
        [NoDirectAccess]
        public async Task<IActionResult> Delete(int? id)
        {
            Category category = await _dataContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
            try
            {
                _dataContext.Categories.Remove(category);
                await _dataContext.SaveChangesAsync();
                _flashMessage.Info("Registro borrado.");
            }
            catch
            {
                _flashMessage.Danger("No se puede borrar la categoría porque tiene registros relacionados.");
            }

            return RedirectToAction(nameof(Index));
        }

        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                return View(new Category());
            }
            else
            {
                Category category = await _dataContext.Categories.FindAsync(id);
                if (category == null)
                {
                    return NotFound();
                }

                return View(category);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (id == 0) //Insert
                    {
                        _dataContext.Add(category);
                        await _dataContext.SaveChangesAsync();
                        _flashMessage.Info("Registro creado.");
                    }
                    else //Update
                    {
                        _dataContext.Update(category);
                        await _dataContext.SaveChangesAsync();
                        _flashMessage.Info("Registro actualizado.");
                    }
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        _flashMessage.Danger("Ya existe una categoría con el mismo nombre.");
                    }
                    else
                    {
                        _flashMessage.Danger(dbUpdateException.InnerException.Message);
                    }
                    return View(category);
                }
                catch (Exception exception)
                {
                    _flashMessage.Danger(exception.Message);
                    return View(category);
                }

                return Json(new { isValid = true, html = ModalHelper.RenderRazorViewToString(this, "_ViewAll", _dataContext.Categories.Include(c => c.ProductCategories).ToList()) });

            }

            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "AddOrEdit", category) });
        }
        //public IActionResult Create()
        //{
        //    return View();
        //}




        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(Category category)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //           await _categoryRepository.AddDataAsync(category);
                    
        //           return RedirectToAction(nameof(Index));
        //        }
        //        catch (DbUpdateException dbUpdateException)
        //        {
        //            if (dbUpdateException.InnerException.Message.Contains("duplicate"))
        //            {
        //                ModelState.AddModelError(string.Empty, "Ya existe una categoría con el mismo nombre.");
        //            }
        //            else
        //            {
        //                ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
        //            }
        //        }
        //        catch (Exception exception)
        //        {
        //            ModelState.AddModelError(string.Empty, exception.Message);
        //        }
        //    }

        //    return View(category);
        //}

        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    Category category = await _categoryRepository.GetOnlyCategoryAsync(id.Value);
        //    if (category == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(category);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, Category category)
        //{
        //    if (id != category.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            await _categoryRepository.UpdateDataAsync(category);
              
        //            return RedirectToAction(nameof(Index));
        //        }
        //        catch (DbUpdateException dbUpdateException)
        //        {
        //            if (dbUpdateException.InnerException.Message.Contains("duplicate"))
        //            {
        //                ModelState.AddModelError(string.Empty, "Ya existe una categoría con el mismo nombre.");
        //            }
        //            else
        //            {
        //                ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
        //            }
        //        }
        //        catch (Exception exception)
        //        {
        //            ModelState.AddModelError(string.Empty, exception.Message);
        //        }
        //    }

        //    return View(category);
        //}

        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    Category category = await _categoryRepository.GetOnlyCategoryAsync(id.Value);
        //    if (category == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(category);
        //}

        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    Category category = await _categoryRepository.GetOnlyCategoryAsync(id.Value);
        //    if (category == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(category);
        //}

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    Category category = await _categoryRepository.GetOnlyCategoryAsync(id);
        //    await _categoryRepository.DeleteDataAsync(category);
        //    return RedirectToAction(nameof(Index));
        //}
    }
}
