using System.ComponentModel.DataAnnotations;

namespace Shopping.Web.Data.Entities
{
    public class ProductImage
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        [Display(Name = "Foto")]
        public Guid ImageId { get; set; }
        [Display(Name = "Picture")]
        public string ImagePath { get; set; }
        //TODO: Pending to change to the correct path
        [Display(Name = "Foto")]
        public string ImageFullPath => ImageId == Guid.Empty
        ? $"https://localhost:7057/images/noimage.png"
        : $"https://shopping4.blob.core.windows.net/products/{ImageId}";

        [Display(Name = "Picture")]
        public string PictureFullPath => ImagePath == string.Empty
           ? $"http://demoshooping.comtecom.com.mx/images/noimage.png"
           : string.Format("http://demoshooping.comtecom.com.mx/{0}", ImagePath.Substring(1));

    }
}
