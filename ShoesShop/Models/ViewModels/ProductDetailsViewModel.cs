using Newtonsoft.Json;

namespace ShoesShop.Models.ViewModels
{
    public class ProductDetailsViewModel
    {
        public ProductModel ProductDetails { get; set; }
        [Required(ErrorMessage = "Please enter comment product")]
        public string Comment { get; set; }
        [Required(ErrorMessage = "Please enter name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter email")]
        public string Email { get; set; }
    }
}
