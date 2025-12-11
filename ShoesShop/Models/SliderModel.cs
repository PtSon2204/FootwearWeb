using System.ComponentModel.DataAnnotations.Schema;
using ShoesShop.Repository.Validation;

namespace ShoesShop.Models
{
    public class SliderModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please name not empty")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please description not empty")]
        public string Description { get; set; }
        public int? Status { get; set; }
        public string Image {  get; set; }
        [NotMapped]
        [FileExtension]
        public IFormFile ImageUpload { get; set; }
    }
}
