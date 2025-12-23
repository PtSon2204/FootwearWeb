using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ShoesShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize( Roles = "admin")]
    public class CouponController : Controller
    {
        private readonly DataContext _dataContext;
        public CouponController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IActionResult> Index()
        {
            var coupon_list = await _dataContext.Coupons.ToListAsync();
            ViewBag.Coupons = coupon_list;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CouponModel coupon) 
        {
            if (ModelState.IsValid)
            {
              _dataContext.Coupons.Add(coupon);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Add successfully";
                return  RedirectToAction("Index");  
            }
            else
            {
                TempData["error"] = "Some have error";
                List<string> errors = new List<string>();   
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }
        }
    }
}
