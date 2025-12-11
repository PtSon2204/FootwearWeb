using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ShoesShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _dataContext;
        private readonly UserManager<AppUserModel> _userManager;

        public HomeController(ILogger<HomeController> logger, DataContext context, UserManager<AppUserModel> userManager)
        {
            _logger = logger;
            _dataContext = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _dataContext.Products
                .Include("Category")
                .Include("Brand")
                .ToListAsync();

            var sliders = _dataContext.Sliders.Where(s => s.Status == 1).ToList();
            ViewBag.Sliders = sliders;

            return View(products);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)] 
        public IActionResult Error(int statuscode)
        {
            if (statuscode == 404)
            {
                return View("NotFound");
            }
            else
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }
        public async Task<IActionResult> Contact()
        {
             var contact = await _dataContext.Contacts.FirstOrDefaultAsync();
            return View(contact);
        }

        [HttpPost]
        public async Task<IActionResult> AddWishlist(long Id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized(new { success = false, message = "User not logged in." });
            }

            var exists = await _dataContext.Wishlists
                .AnyAsync(w => w.ProductId == Id && w.UserId == user.Id);

            if (exists)
            {
                return Ok(new { success = false, message = "Product is already in wishlist." });
            }

            var wishlistProduct = new WishlistModel
            {
                ProductId = Id,
                UserId = user.Id
            };

            _dataContext.Wishlists.Add(wishlistProduct); 

            try
            {
                await _dataContext.SaveChangesAsync();
                return Ok(new { success = true, message = "Add to wishlist successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding to wishlist.");
                return StatusCode(500, new { success = false, message = "An error occurred while adding to wishlist." });
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddCompare(long Id) 
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized(new { success = false, message = "User not logged in." });
            }

            var exists = await _dataContext.Compares
                .AnyAsync(c => c.ProductId == Id && c.UserId == user.Id);

            if (exists)
            {
                return Ok(new { success = false, message = "Product is already in compare list." });
            }

            var compareProduct = new CompareModel
            {
                ProductId = Id,
                UserId = user.Id
            };

            _dataContext.Compares.Add(compareProduct);

            try
            {
                await _dataContext.SaveChangesAsync();
                return Ok(new { success = true, message = "Add to compare successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding to compare list.");
                return StatusCode(500, new { success = false, message = "An error occurred while adding to compare list." });
            }
        }
    }
}
