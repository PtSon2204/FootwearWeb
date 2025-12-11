using System.ComponentModel.DataAnnotations.Schema;

namespace ShoesShop.Models
{
    public class ContactModel
    {
        [Key]
        [Required(ErrorMessage = "Please enter name website")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter map")]
        public string Map { get; set; }
        [Required(ErrorMessage = "Please enter email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter phone")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Please enter info contact")]
        public string Description { get; set; }
        public string LogoImg {  get; set; }
        [NotMapped]
        [FileExtension]
        public IFormFile? ImageUpload { get; set; }
    }
}
