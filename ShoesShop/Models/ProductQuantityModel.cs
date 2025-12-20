using System.Diagnostics.Contracts;

namespace ShoesShop.Models
{
    public class ProductQuantityModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Request do not empty quantity")]
        public int Quantity { get; set; }
        public long ProductId { get; set; }
        public DateTime DateCreated { get; set; }
       
    }
}
