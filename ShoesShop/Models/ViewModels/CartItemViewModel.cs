namespace ShoesShop.Models.ViewModels
{
    public class CartItemViewModel
    {
        public List<CartItemModel> Items { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal ShippingCost { get; set; }
        public string CouponCode { get; set; }
    }
}
