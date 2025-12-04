using Microsoft.AspNetCore.Mvc;

namespace ShoesShop.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
