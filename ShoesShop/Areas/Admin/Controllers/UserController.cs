using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ShoesShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]

    public class UserController : Controller
    {
        private readonly UserManager<AppUserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DataContext _dataContext;
        public UserController (DataContext dataContext, RoleManager<IdentityRole> roleManager,  UserManager<AppUserModel> userManager)
        {
            _dataContext = dataContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var usersWithRoles = await (
                       from u in _dataContext.Users
                       join ur in _dataContext.UserRoles on u.Id equals  ur.       UserId       into        userRolesGroup
                       from ur in userRolesGroup.DefaultIfEmpty()   // LEFT JOIN
                       join r in _dataContext.Roles on ur.RoleId equals r.  Id      into      rolesGroup
                       from r in rolesGroup.DefaultIfEmpty()       // LEFT JOIN
                       select new
                       {
                           User = u,
                           RoleName = r != null ? r.Name : "Null"
                                     }
                       ).ToListAsync();
            return View(usersWithRoles);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "Name");
            return View(new AppUserModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppUserModel user)
        {
            if (ModelState.IsValid)
            {
                var createUserResult = await _userManager.CreateAsync(user, user.PasswordHash);
                if (createUserResult.Succeeded)
                {
                    var createUser = await _userManager.FindByEmailAsync(user.Email);
                    var userId = createUser.Id;
                    var role = await _roleManager.FindByIdAsync(user.RoleId);

                    var addToRoleResult = await _userManager.AddToRoleAsync(createUser, role.Name);
                    if (!addToRoleResult.Succeeded)
                    {
                        return BadRequest(string.Join("\n", addToRoleResult.Errors.Select(e => e.Description)));
                    }
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    string errors = string.Join("\n", createUserResult.Errors.Select(e => e.Description));
                    return BadRequest(errors);
                }

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
        public async Task<IActionResult> Edit(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(Id);

            if (user == null)
            {
                return NotFound();
            }

            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "Name");

            return View(user);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AppUserModel model)
        {
            var existingUser = await _userManager.FindByIdAsync(model.Id);
            if (existingUser == null)
            {
                return NotFound();  
            }
            if (ModelState.IsValid)
            {
                existingUser.UserName = model.UserName;
                existingUser.Email = model.Email;
                existingUser.PhoneNumber = model.PhoneNumber;

                var currentRoles = await _userManager.GetRolesAsync(existingUser);

                // 4. Lấy RoleName từ RoleId
                var newRoleName = await _roleManager.Roles
                    .Where(r => r.Id == model.RoleId)
                    .Select(r => r.Name)
                    .FirstOrDefaultAsync();

                if (newRoleName == null)
                {
                    TempData["error"] = "Role không tồn tại!";
                    return View(model);
                }

                // 5. Xóa role cũ
                if (currentRoles.Count > 0)
                {
                    var removeResult = await _userManager.RemoveFromRolesAsync(existingUser, currentRoles);
                    if (!removeResult.Succeeded)
                    {
                        TempData["error"] = "Không thể xóa role cũ!";
                        return View(model);
                    }
                }

                // 6. Thêm role mới
                var addResult = await _userManager.AddToRoleAsync(existingUser, newRoleName);
                if (!addResult.Succeeded)
                {
                    TempData["error"] = "Không thể thêm role mới!";
                    return View(model);
                }


                var updateUserResult = await _userManager.UpdateAsync(existingUser);
                if (updateUserResult.Succeeded)
                {
                    TempData["success"] = "Update successfully!";
                    return RedirectToAction("Index", "User");
                } 
                else
                {
                    string errors = string.Join("\n", updateUserResult.Errors.Select(e => e.Description));
                    return BadRequest(errors);
                }
            }
            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "Name");

            TempData["error"] = "Model validation failed";
            var error = ModelState.Values.SelectMany(e => e.Errors.Select(e => e.ErrorMessage).ToList());
            string errorMessage = string.Join("\n", error);
            return View(existingUser);
        }
        public async Task<IActionResult> Delete(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return NotFound();
            }
            var userGetById = await _userManager.FindByIdAsync(Id);
            
            if (userGetById == null)
            {
                return NotFound();
            }
            var deleteResult = await _userManager.DeleteAsync(userGetById);
            if (!deleteResult.Succeeded)
            {
                return View("Error");
            }
            TempData["success"] = "Remove user successfully";
            return RedirectToAction("Index", "User");
        }
    }
}
