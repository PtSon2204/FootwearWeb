

using System.ComponentModel.DataAnnotations.Schema;

namespace ShoesShop.Models
{
    public class RatingModel
    {
        [Key]
        public int Id { get; set; }
        public long ProductId { get; set; }
        [Required(ErrorMessage = "PLease enter review product")]
        public string Comment { get; set; }
        [Required(ErrorMessage = "PLease enter name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "PLease enter email")]
        public string Email { get; set; }
        public string Star {  get; set; }

        [ForeignKey("ProductId")]
        public ProductModel Product { get; set; }
    }
}
