namespace ShoesShop.Models.Momo
{
    public class MomoExecuteResponseModel
    {
        public string FullName { get; set; }
        public string? OrderId { get; set; }
        public string? Amount { get; set; }
        public string? OrderInfo { get; set; }
        public string? ResultCode { get; set; } // Thêm cái này để kiểm tra lỗi
        public string? Message { get; set; }    // Thêm cái này để hiện thông báo từ MoMo
    }
}
