using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ShoesShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class BrandController : Controller
    {
        private readonly DataContext _dataContext;
        public BrandController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Brands.OrderByDescending(b => b.Id).ToListAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BrandModel brand)
        {
            if (ModelState.IsValid)
            {
                TempData["success"] = "Model created successfully";
                brand.Slug = brand.Name.Replace(" ", "-");
                var slug = await _dataContext.Brands.FirstOrDefaultAsync(b => b.Slug == brand.Slug);

                if (slug != null)
                {
                    ModelState.AddModelError("", "Brand already exists!");
                    return View(brand);
                }

                _dataContext.Brands.Add(brand);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Add brand successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Model some value error";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }
        }

        public async Task<IActionResult> Edit(int Id)
        {
            return View(await _dataContext.Brands.FirstOrDefaultAsync(b => b.Id == Id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BrandModel brand)
        {
            var exists_brand = await _dataContext.Brands.FindAsync(brand.Id);
            if (ModelState.IsValid)
            {
                brand.Slug = brand.Name.Replace(" ", "-");

                var slug = await _dataContext.Brands
                    .FirstOrDefaultAsync(b => b.Slug == brand.Slug && b.Id != brand.Id);

                if (slug != null)
                {
                    ModelState.AddModelError("", "Slug already exists!");
                    return View(brand);
                }

                exists_brand.Name = brand.Name;
                exists_brand.Description = brand.Description;
                exists_brand.Status = brand.Status;
                _dataContext.Brands.Update(exists_brand);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Update successfully";
                return RedirectToAction("Index");   
            }
            else
            {
                TempData["error"] = "Model some value error";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }
        }

        public async Task<IActionResult> Delete(int Id)
        {
            BrandModel brand = await _dataContext.Brands.FindAsync(Id);

            _dataContext.Brands.Remove(brand);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Remove successfully!";
            return RedirectToAction("Index");
        }
    }
}
