using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ShoesShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class ShipController : Controller
    {
        private readonly DataContext _dataContext;
        public ShipController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IActionResult> Index()
        {
            var shippingList = await _dataContext.Ships.ToListAsync();
            ViewBag.Shippings = shippingList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> StoreShipping(ShipModel ship, string provinces, string districts, string communues, decimal price)
        {
            var shipping = new ShipModel
            {
                City = provinces,
                District = districts,
                Ward = communues,
                Price = price,
                DateCreated = DateTime.Now
            };

            var existingShip = await _dataContext.Ships.AnyAsync(x => x.City == provinces && x.District == districts && x.Ward == communues);
            if (existingShip) return Ok(new { duplicate = true });

            _dataContext.Ships.Add(shipping);
            await _dataContext.SaveChangesAsync();
            return Ok(new { success = true });
        }

        public async Task<IActionResult> Delete(int Id)
        {
            ShipModel shipping = await _dataContext.Ships.FindAsync(Id);

            _dataContext.Ships.Remove(shipping);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Shipping removed!";
            return RedirectToAction("Index");
        }
    }
}
