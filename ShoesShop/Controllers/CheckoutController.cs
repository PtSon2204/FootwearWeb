using System.Security.Claims;

namespace ShoesShop.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IEmailSender _emailSender;
        public CheckoutController(IEmailSender emailSender, DataContext dataContext)
        {
            _dataContext = dataContext;
            _emailSender = emailSender;
        }
        public async Task<IActionResult> Checkout()
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
                orderItem.UserName = userEmail;
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

                return RedirectToAction("Index", "Cart");
            }
        }
    }
}
