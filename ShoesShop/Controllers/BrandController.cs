namespace ShoesShop.Controllers
{
    public class BrandController : Controller
    {
        private readonly DataContext _dataContext;

        public BrandController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IActionResult> Index(string Slug = "")
        {
            BrandModel model = _dataContext.Brands.Where(b => b.Slug == Slug).FirstOrDefault(); 

            if (model == null)
            {
                return RedirectToAction("Index");
            }

            var productsByBrand = _dataContext.Products.Where(p => p.Id == model.Id);
            return View(await productsByBrand.OrderByDescending(x => x.Id).ToListAsync());
        }
    }
}
