using Microsoft.AspNetCore.Mvc.Rendering;
using Shopping.Web.Data.Entities;

namespace Shopping.Web.Applications.Interfaces
{
    public interface IProductoRepositorio : IRepositorio<Product>
    {
        void Actualizar(Product producto);
        IEnumerable<SelectListItem> ObtenerTodosDropdownList(string obj);
    }
}
