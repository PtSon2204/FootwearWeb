namespace ShoesShop.Models
{
    public class CouponModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Not empty name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Not empty description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Not empty quantity")]
        public int Quantity { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateExpired { get; set; }
        public int? Status { get; set; }
    }
}
