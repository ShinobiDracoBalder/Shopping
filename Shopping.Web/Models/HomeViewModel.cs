using Shooping.Common.Objects;
using Shopping.Web.Data.Entities;

namespace Shopping.Web.Models
{
    public class HomeViewModel
    {
        public PaginatedList<Product> Products { get; set; }
        //public ICollection<ProductsHomeViewModel> Products { get; set; }

        //public ICollection<Product> Products { get; set; }

        public ICollection<Category> Categories { get; set; }

        public float Quantity { get; set; }
    }
}
