using Microsoft.AspNetCore.Mvc;
using ShoesShop.Models.ViewModels;

namespace ShoesShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly DataContext _dataContext;

        public ProductController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Search(string searchTerm)
        {
            var products = await _dataContext.Products.Where(p => p.Name.ToLower().Contains(searchTerm.ToLower()) || p.Description.ToLower().Contains(searchTerm.ToLower())).ToListAsync();

            if (products == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Keyword = searchTerm;
            return View(products);
        }

        public async Task<IActionResult> Details(long Id)
        {
            var productById = _dataContext.Products.Include(p => p.Rating).Where(p => p.Id == Id).FirstOrDefault();

            var relatedProducts = await _dataContext.Products.Where(p => p.CategoryId == productById.CategoryId && p.Id != productById.Id).Take(4).ToListAsync();

            ViewBag.RelatedProducts = relatedProducts;

            var viewModel = new ProductDetailsViewModel
            {
                ProductDetails = productById,
            };
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CommentProduct(RatingModel rating)
        {
            if (ModelState.IsValid)
            {
                var ratingEntity = new RatingModel
                {
                    ProductId = rating.ProductId,
                    Name = rating.Name,
                    Email = rating.Email,
                    Comment = rating.Comment,
                    Star = rating.Star
                };

                _dataContext.Ratings.Add(ratingEntity);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Add rating successfully!";
                return Redirect(Request.Headers.Referer.ToString());//trả về trang trước đó 
            }
            else
            {
                TempData["error"] = "Model some value error";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return RedirectToAction("Details", new { id = rating.ProductId }); //quay ve trang detail voi id = ?
            }
        }
    }
}
