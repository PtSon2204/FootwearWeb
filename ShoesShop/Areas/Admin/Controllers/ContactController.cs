using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ShoesShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class ContactController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ContactController (DataContext dataContext, IWebHostEnvironment webHostEnvironment)
        {
            _dataContext = dataContext;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var contact = _dataContext.Contacts.ToList();
            return View(contact);
        }

        public async Task<IActionResult> Edit()
        {
            ContactModel contact = await _dataContext.Contacts.FirstOrDefaultAsync();
            return View(contact);
        }
        [HttpPost]
        [ValidateAntiForgeryToken] //bảo vệ dữ liệu 
        public async Task<IActionResult> Edit(ContactModel contact)
        {
            var existed_contact = _dataContext.Contacts.FirstOrDefault();

            if (ModelState.IsValid)
            {
                //code them du lieu
                TempData["success"] = "Model update successfully!";

                if (contact.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/logoContact");

                    if (!Directory.Exists(uploadsDir))
                        Directory.CreateDirectory(uploadsDir);

                    // Xóa ảnh cũ nếu có
                    if (!string.IsNullOrEmpty(existed_contact.LogoImg))
                    {
                        string oldfilePath = Path.Combine(uploadsDir, existed_contact.LogoImg);

                        if (System.IO.File.Exists(oldfilePath))
                        {
                            System.IO.File.Delete(oldfilePath);
                        }
                    }

                    // Upload ảnh mới
                    string imageName = Guid.NewGuid().ToString() + "_" + contact.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);

                    using (var fs = new FileStream(filePath, FileMode.Create))
                    {
                        await contact.ImageUpload.CopyToAsync(fs);
                    }

                    existed_contact.LogoImg = imageName;
                }
                //update
                existed_contact.Name = contact.Name;
                existed_contact.Description = contact.Description;
                existed_contact.Map = contact.Map;
                existed_contact.Email = contact.Email;
          

                _dataContext.Contacts.Update(existed_contact);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Update product successfully!";
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
    }
}
