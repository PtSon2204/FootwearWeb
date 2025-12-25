using Microsoft.AspNetCore.Mvc;
using ShoesShop.Services;

namespace ShoesShop.Controllers
{
    public class PaymentController : Controller
    {
        private IMomoService _momoService;
        //private readonly IVnPayService _vnPayService;
        public PaymentController(IMomoService momoService)
        {
            _momoService = momoService;

        }
        [HttpPost]
        public async Task<IActionResult> CreatePaymentUrl(OrderInfoModel model)
        {
            var response = await _momoService.CreatePaymentAsync(model);
            return Redirect(response.PayUrl);
        }

    }
}
