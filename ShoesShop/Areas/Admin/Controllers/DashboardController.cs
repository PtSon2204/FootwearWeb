using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ShoesShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class DashboardController : Controller
    {
        private readonly DataContext _dataContext;
        public DashboardController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IActionResult> Index()
        {
            var count_product = await _dataContext.Products.CountAsync();
            var count_order = await _dataContext.Orders.CountAsync();
            var count_category = await _dataContext.Categories.CountAsync();
            var count_user = await _dataContext.Users.CountAsync();
            ViewBag.CountProduct = count_product;
            ViewBag.CountOrder = count_order;
            ViewBag.CountCategory = count_category;
            ViewBag.CountUser = count_user;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetChartData()
        {
            var data = await _dataContext.Statistical.OrderBy(s => s.DateCreated)
               .Select(s => new
               {
                   date = s.DateCreated.ToString("yyyy-MM-dd"),
                   sold = s.Sold,
                   quantity = s.Quantity,
                   revenue = s.Revenue,
                   profit = s.Profit,
               }).ToListAsync();

            return Json(data);
        }
        [HttpPost]
        public async Task<IActionResult> GetChartDataBySelect(DateTime startDate, DateTime endDate)
        {
            var data = await _dataContext.Statistical
              .Where(s => s.DateCreated >= startDate && s.DateCreated <= endDate)
              .OrderBy(s => s.DateCreated)
              .Select(s => new
              {
                  date = s.DateCreated.ToString("yyyy-MM-dd"),
                  sold = s.Sold,
                  quantity = s.Quantity,
                  revenue = s.Revenue,
                  profit = s.Profit,
              }).ToListAsync();
              
                  return Json(data);
        }

        [HttpPost] 
        public async Task<IActionResult> FilterData(DateTime? fromDate, DateTime? toDate)
        {
            var query = _dataContext.Statistical.AsQueryable();

            if (fromDate.HasValue)
            {
                query = query.Where(s => s.DateCreated >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(s => s.DateCreated <= toDate.Value);
            }

            var stats = await query.OrderBy(s => s.DateCreated).ToListAsync();

            var result = stats.Select(s => new
            {
                date = s.DateCreated.ToString("yyyy-MM-dd"),
                sold = s.Sold,
                quantity = s.Quantity,
                revenue = s.Revenue,
                profit = s.Profit
            });

            return Json(result);
        }
    }
}
