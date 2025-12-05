using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ShoesShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class RoleController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleController(DataContext dataContext, RoleManager<IdentityRole> roleManager)
        {
            _dataContext = dataContext;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Roles.OrderByDescending(r => r.Id).ToListAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IdentityRole role)
        {
            if (ModelState.IsValid)
            {
                if (!_roleManager.RoleExistsAsync(role.Name).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new IdentityRole(role.Name)).GetAwaiter().GetResult();
                }
                TempData["success"] = "Add role successfully!";
                return RedirectToAction("Index", "Role");
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

        public async Task<IActionResult> Delete(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return NotFound();
            }

            var role = await _roleManager.FindByIdAsync(Id);

            if (role == null)
            {
                return NotFound();
            }
            var removeResult = await _roleManager.DeleteAsync(role);
            if (!removeResult.Succeeded)
            {
                TempData["error"] = "An error occurred while deleting the role";
                return View("Error");
            }
            TempData["success"] = $"Delete {role} successfully";
            return RedirectToAction("Index", "Role");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return NotFound();
            }
            var role =  await _roleManager.FindByIdAsync(Id);
            if (role == null)
            {
                return NotFound();
            }
            
            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] 
        
        public async Task<IActionResult> Edit(IdentityRole model) 
        {
            if (string.IsNullOrEmpty(model.Id))
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(model.Id);   
                if (role == null)
                {
                    return NotFound();
                }
                role.Name = model.Name;
                await _roleManager.UpdateAsync(role);
                TempData["success"] = "Role updated successfully!";
                return RedirectToAction("Index", "Role");
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
