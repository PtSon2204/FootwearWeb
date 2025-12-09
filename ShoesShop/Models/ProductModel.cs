using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShoesShop.Repository.Validation;

namespace ShoesShop.Models
{
    public class ProductModel
    {
        [Key]
        public long Id { get; set; }
        [Required, MinLength(4, ErrorMessage = "Product name required")]
        public string Name { get; set; }

        public string Slug { get; set; }
        [Required, MinLength(4, ErrorMessage = "Product description required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Product price required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }

        public CategoryModel Category { get; set; }
        public BrandModel Brand { get; set; }
        public RatingModel Rating { get; set; }
        public string Image { get; set; }
        [NotMapped]
        [FileExtension]
        public IFormFile ImageUpload { get; set; }
    }
}
