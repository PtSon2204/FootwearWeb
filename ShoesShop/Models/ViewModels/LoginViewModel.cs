using System.ComponentModel.DataAnnotations;

namespace ShoesShop.Models.ViewModels
{
    public class LoginViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter Username!")]
        public string Username { get; set; }
        [DataType(DataType.Password), Required(ErrorMessage = " Please enter Password")]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}
