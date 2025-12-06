using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ShoesShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly DataContext _dataContext;
        public OrderController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Orders.OrderByDescending(o => o.Id).ToListAsync());
        } 

        public async Task<IActionResult> ViewOrder(string ordercode)
        {
            var orderDetails = await _dataContext.OrderDetails.Include(od => od.Product).Where(od => od.OrderCode == ordercode).ToListAsync();
            return View(orderDetails);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrder (string ordercode, int status)
        {
            var order = await _dataContext.Orders.FirstOrDefaultAsync(o => o.OrderCode == ordercode);

            if (order == null)
            {
                return NotFound();
            }

            order.Status = status;
            try
            {
                await _dataContext.SaveChangesAsync();
                return Ok(new { success = true, message = "Order status updated successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occured while updating the order status.");
            }
        }

        //[HttpGet]
        //public async Task<IActionResult> Delete(string ordercode)
        //{
        //    var order = await _dataContext.Orders.FirstOrDefaultAsync(o => o.OrderCode == ordercode);

        //    if (order == null)
        //    {
        //        return NotFound();
        //    }
        //}

    }
}
