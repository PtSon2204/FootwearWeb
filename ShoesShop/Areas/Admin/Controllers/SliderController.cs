using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ShoesShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class SliderController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public SliderController(DataContext dataContext, IWebHostEnvironment webHostEnvironment)
        {
            _dataContext = dataContext;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Sliders.OrderByDescending(s => s.Id).ToListAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderModel slider)
        {
            if (ModelState.IsValid)
            {
                if (slider.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/sliders");
                    string imageName = Guid.NewGuid().ToString() + "_" + slider.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await slider.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    slider.Image = imageName;
                }

                _dataContext.Sliders.Add(slider);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Add slider successfully!";
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

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            SliderModel slider = await _dataContext.Sliders.FindAsync(Id);
            return View(slider);
        }


        [HttpPost]
        [ValidateAntiForgeryToken] //bảo vệ dữ liệu 
        public async Task<IActionResult> Edit(SliderModel slider)
        {
            var existed_slider = _dataContext.Sliders.Find(slider.Id);

            if (ModelState.IsValid)
            {

                if (slider.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/sliders");
                    string imageName = Guid.NewGuid().ToString() + "_" + slider.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);

                    //delete old picture
                    string oldfilePath = Path.Combine(uploadsDir, existed_slider.Image);

                    try
                    {
                        if (System.IO.File.Exists(oldfilePath))
                        {
                            System.IO.File.Delete(oldfilePath);
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "An error occurred while deleting the product image.");
                    }

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await slider.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    existed_slider.Image = imageName;
                }
                //update
                existed_slider.Name = slider.Name;
                existed_slider.Description = slider.Description;
                existed_slider.Status = slider.Status;

                _dataContext.Sliders.Update(existed_slider);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Update slider successfully!";
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
            SliderModel slider = await _dataContext.Sliders.FindAsync(Id);

            if (slider == null)
            {
                return NotFound();
            }
            string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/sliders");
            string oldfilePath = Path.Combine(uploadsDir, slider.Image);

            try
            {
                if (System.IO.File.Exists(oldfilePath))
                {
                    System.IO.File.Delete(oldfilePath);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while deleting the product image.");
            }

            _dataContext.Sliders.Remove(slider);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Slider deleted";
            return RedirectToAction("Index");
        }
    }
}
