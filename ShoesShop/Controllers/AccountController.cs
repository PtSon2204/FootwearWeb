using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoesShop.Models.ViewModels;

namespace ShoesShop.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUserModel> _userManage;

        private SignInManager<AppUserModel> _signInManager;
        private readonly IEmailSender _emailSender;

        public AccountController (IEmailSender emailSender , SignInManager<AppUserModel> signInManager, UserManager<AppUserModel> userManager)
        {
            _userManage = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl});
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVm)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(loginVm.Username, loginVm.Password, false, false);

                if (result.Succeeded)
                {
                    var userEmail = User.FindFirstValue(ClaimTypes.Email);
                    TempData["success"] = "Login successfully!";
                    var receiver = userEmail;
                    var subject = "Login on equipment successfully!";
                    var message = "Have a nice day!";

                    await _emailSender.SendEmailAsync(receiver, subject, message);
                    return Redirect(loginVm.ReturnUrl ?? "/");
                }
                ModelState.AddModelError("", "Invalid Username and Password");
            }
            return View(loginVm);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserModel user)
        {
            if (ModelState.IsValid)
            {
                AppUserModel newUser = new AppUserModel { UserName = user.Username, Email = user.Email};
                IdentityResult result = await _userManage.CreateAsync(newUser, user.Password);

                if (result.Succeeded)
                {
                    TempData["success"] = "Create user successfully!";
                    return Redirect("/account/login");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(user);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync(); //thoát
            return RedirectToAction("Index", "Home");
        }
    }
}
