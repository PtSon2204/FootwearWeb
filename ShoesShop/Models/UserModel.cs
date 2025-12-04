using System.ComponentModel.DataAnnotations;

namespace ShoesShop.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter Username!")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Please enter Email!"), EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password), Required(ErrorMessage = " Please enter Password")]
        public string Password { get; set; }
    }
}
