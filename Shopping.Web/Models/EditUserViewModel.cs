﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Shopping.Web.Models
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

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

        [Display(Name = "Dirección")]
        [MaxLength(200, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Address { get; set; }

        [Display(Name = "Teléfono")]
        [MaxLength(20, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Foto")]
        public Guid ImageId { get; set; }

        [Display(Name = "Picture")]
        public string PicturePath { get; set; }

        [Display(Name = "Foto")]
        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://localhost:7057/images/noimage.png"
            : $"https://shoppingzulu.blob.core.windows.net/users/{ImageId}";

        //[Display(Name = "Picture")]
        //public string PictureFullPath => PicturePath == string.Empty
        //  ? $"https://localhost:7057/images/noimage.png"
        //  : $"https://localhost:7057/users/{PicturePath}";

        [Display(Name = "Picture")]
        public string PictureFullPath => PicturePath == string.Empty
          ? $"https://localhost:7144/images/noimage.png"
          : string.Format("https://localhost:7144/{0}", PicturePath.Substring(1));


        [Display(Name = "Image")]
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "País")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes de seleccionar un país.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int CountryId { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }

        [Display(Name = "Departmento / Estado")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes de seleccionar un Departamento / Estado.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int StateId { get; set; }

        public IEnumerable<SelectListItem> States { get; set; }

        [Display(Name = "Ciuadad")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes de seleccionar una ciudad.")]
        public int CityId { get; set; }

        public IEnumerable<SelectListItem> Cities { get; set; }
    }
}
