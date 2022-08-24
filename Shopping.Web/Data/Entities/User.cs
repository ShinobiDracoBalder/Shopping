using Microsoft.AspNetCore.Identity;
using Shooping.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace Shopping.Web.Data.Entities
{
    public class User : IdentityUser
    {
        [Display(Name = "Documento")]
        [MaxLength(20, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Document { get; set; }

        [Display(Name = "Nombres")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string FirstName { get; set; }

        [Display(Name = "Apellidos")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string LastName { get; set; }

        [Display(Name = "Ciudad")]
        public City City { get; set; }

        [Display(Name = "Dirección")]
        [MaxLength(200, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Address { get; set; }

        [Display(Name = "Foto")]
        public Guid ImageId { get; set; }

        [Display(Name = "Picture")]
        public string PicturePath { get; set; }


        [Display(Name = "Foto")]
        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://localhost:7144/images/noimage.png"
            : $"https://shoppingzulu.blob.core.windows.net/users/{ImageId}";

        [Display(Name = "Picture")]
        //public string PictureFullPath => PicturePath == string.Empty
        //   ? $"https://localhost:7144/images/noimage.png"
        //   : $"https://localhost:7144/users/{PicturePath}";

        public string PictureFullPath => PicturePath == string.Empty
          ? $"https://localhost:7144/images/noimage.png"
          : string.Format("https://localhost:7144/{0}", PicturePath.Substring(1));

        public ICollection<Sale> Sales { get; set; }

        [Display(Name = "Tipo de usuario")]
        public UserType UserType { get; set; }

        [Display(Name = "Usuario")]
        public string FullName => $"{FirstName} {LastName}";

        [Display(Name = "Usuario")]
        public string FullNameWithDocument => $"{FirstName} {LastName} - {Document}";
    }
}
