using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

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
            var orderDetails = await _dataContext.OrderDetails
         .Include(od => od.Product)
         .Where(od => od.OrderCode == ordercode)
         .ToListAsync();

            var order = await _dataContext.Orders.FirstOrDefaultAsync(o => o.OrderCode == ordercode);

            if (order == null) return NotFound();

            ViewBag.ShippingCost = order.ShippingCost;
            ViewBag.CurrentStatus = order.Status;


            return View(orderDetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrder(string ordercode, int status)
        {
            var order = await _dataContext.Orders.FirstOrDefaultAsync(o => o.OrderCode == ordercode);

            if (order == null)
            {
                return NotFound(new { success = false, message = $"Order with code {ordercode} not found." });
            }

            order.Status = status;
            if (status == 3)
            {
                var detailOrders = await _dataContext.OrderDetails.Include(od => od.Product).Where(od => od.OrderCode == order.OrderCode).Select(od => new
                {
                    od.Quantity,
                    od.Product.Price,
                    od.Product.CapitalPrice
                }).ToListAsync();

                var statisticModel = await _dataContext.Statistical.FirstOrDefaultAsync(s => s.DateCreated.Date == order.CreateDate.Date);

                if (statisticModel != null)
                {
                    foreach (var orderDetail in detailOrders)
                    {
                        statisticModel.Quantity += 1;
                        statisticModel.Sold += orderDetail.Quantity;
                        statisticModel.Revenue += orderDetail.Quantity * orderDetail.Price;
                        statisticModel.Profit += orderDetail.Price - orderDetail.CapitalPrice;
                    }
                    _dataContext.Update(statisticModel);
                }
                else
                {
                    int new_quantity = 0;
                    int new_sold = 0;
                    decimal new_profit = 0;
                    foreach (var orderDetail in detailOrders)
                    {
                        new_quantity += 1;
                        new_sold += orderDetail.Quantity;
                        new_profit += orderDetail.Price - orderDetail.CapitalPrice;

                        statisticModel = new StatisticalModel
                        {
                            DateCreated = order.CreateDate,
                            Quantity = new_quantity,
                            Sold = new_sold,
                            Revenue = orderDetail.Quantity * orderDetail.Price,
                            Profit = new_profit
                        };
                    }
                    _dataContext.Update(statisticModel);
                }
            }
            try
            {
                await _dataContext.SaveChangesAsync();
                return Ok(new { success = true, message = "Order status updated successfully!" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, message = "An error occurred while updating the order status." });
            }
        }
    }
}