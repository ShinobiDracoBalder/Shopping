using Microsoft.AspNetCore.Mvc.Rendering;
using Shopping.Web.Applications.Interfaces;
using Shopping.Web.Data;
using Shopping.Web.Data.Entities;

namespace Shopping.Web.Applications.Repositories
{
    public class ProductoRepositorio : Repositorio<Product>, IProductoRepositorio
    {
        private readonly DataContext _dataContext;

        public ProductoRepositorio(DataContext dataContext) : base(dataContext)
        {
            _dataContext = dataContext;
        }
        public void Actualizar(Product producto)
        {
            _dataContext.Update(producto);
        }

        public IEnumerable<SelectListItem> ObtenerTodosDropdownList(string obj)
        {
            //if (obj == WC.CategoriaNombre)
            //{
            return _dataContext.Categories.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });
            //}
            //if (obj == WC.TipoAplicacionNombre)
            //{
            //return _dataContext.TipoAplicacion.Select(c => new SelectListItem
            //{
            //    Text = c.Nombre,
            //    Value = c.Id.ToString()
            //});
            //}
            return null;
        }
    }
}
