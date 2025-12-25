using System.Security.Claims;
using Newtonsoft.Json;
using ShoesShop.Migrations;
using ShoesShop.Services;

namespace ShoesShop.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IEmailSender _emailSender;
        private readonly IMomoService _momoService;
        public CheckoutController(IEmailSender emailSender, DataContext dataContext, IMomoService momoService)
        {
            _dataContext = dataContext;
            _emailSender = emailSender;
            _momoService = momoService;
        }
        [HttpGet]
        public async Task<IActionResult> Checkout(string OrderId)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                var orderCode = Guid.NewGuid().ToString();
                var orderItem = new OrderModel();
                orderItem.OrderCode = orderCode;

                var shippingPriceCookie = Request.Cookies["ShippingPrice"];
                decimal shippingPrice = 0;

                var coupon_code = Request.Cookies["CouponTitle"];

                if (shippingPriceCookie != null)
                {
                    var shippingPriceJson = shippingPriceCookie;
                    shippingPrice = JsonConvert.DeserializeObject<decimal>(shippingPriceJson);
                }
                orderItem.ShippingCost = shippingPrice;
                orderItem.CouponCode = coupon_code; 
                orderItem.UserName = userEmail;
                if (OrderId != null)
                {
                    orderItem.PaymentMethod = OrderId;
                }
                else
                {
                    orderItem.PaymentMethod = "COD";
                }
                    orderItem.Status = 1;
                orderItem.CreateDate = DateTime.Now;
                _dataContext.Orders.Add(orderItem);
                _dataContext.SaveChanges();
                TempData["success"] = "Create order successfully";

                List<CartItemModel> cartItem = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
                foreach (var item in cartItem)
                {
                    var orderDetails = new OrderDetails();
                    orderDetails.UserName = userEmail;
                    orderDetails.OrderCode = orderCode;
                    orderDetails.ProductId = item.ProductId;
                    orderDetails.Price = item.Price;
                    orderDetails.Quantity = item.Quantity;

                    var product = await _dataContext.Products.Where(p => p.Id == item.ProductId).FirstOrDefaultAsync();
                    product.Quantity -= item.Quantity;
                    product.Sold += item.Quantity;
                    _dataContext.Update(product);

                    _dataContext.OrderDetails.Add(orderDetails);
                    _dataContext.SaveChanges();
                }
                HttpContext.Session.Remove("Cart");
                //Send mail order successfully
                var receiver = userEmail;
                var subject = "Order on equipment successfully!";
                var message = "Order successfully. Have a nice day!";

                await _emailSender.SendEmailAsync(receiver, subject, message);

                TempData["success"] = "Checkout successfully, please wait for order approval";

                return RedirectToAction("History", "Account");
            }
        }

        public async Task<IActionResult> PaymentCallBack(Models.MomoInfoModel model)
        {
            var response = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);
            var requestQuery = HttpContext.Request.Query;

            if (requestQuery["resultCode"] == 0)
            {
                var newMomoInsert = new Models.Momo.MomoInfoModel
                {
                    OrderId = requestQuery["orderId"],
                    FullName = User.FindFirstValue(ClaimTypes.Email),
                    Amount = decimal.Parse(requestQuery["Amount"]),
                    OrderInfo = requestQuery["orderInfo"],
                    DatePaid = DateTime.Now
                };
                _dataContext.Add(newMomoInsert);
                await _dataContext.SaveChangesAsync();

                await Checkout(requestQuery["orderId"]); 
            }
            else
            {
                TempData["success"] = "Cancel transaction with Momo";
                return RedirectToAction("Index", "Cart");
            }

                return View(response);
        }
    }
}
