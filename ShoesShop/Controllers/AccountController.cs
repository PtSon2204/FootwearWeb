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
        private readonly DataContext _dataContext;
        private SignInManager<AppUserModel> _signInManager;
        private readonly IEmailSender _emailSender;

        public AccountController (IEmailSender emailSender , SignInManager<AppUserModel> signInManager, UserManager<AppUserModel> userManager, DataContext dataContext)
        {
            _userManage = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _dataContext = dataContext;
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

        public async Task<IActionResult> NewPass(string email, string token) 
        {
            var checkUser = await _userManage.Users
                .Where(u => u.Email == email)
                .Where(u => u.Token == token).FirstOrDefaultAsync();

            if (checkUser != null)
            {
                ViewBag.Email = email;
                ViewBag.Token = token;
                return View();
            }
            else
            {
                TempData["error"] = "Email not found or token is invalid";
                return RedirectToAction("ForgetPass", "Account");
            }
        }
        public async Task<IActionResult> UpdateNewPassword(AppUserModel user, string token)
        {
            var checkUser = await _userManage.Users
               .Where(u => u.Email == user.Email)
               .Where(u => u.Token == user.Token).FirstOrDefaultAsync();

            if (checkUser != null)
            {
                string newToken = Guid.NewGuid().ToString();
                var passwordHasher = new PasswordHasher<AppUserModel>();
                var passwordHash = passwordHasher.HashPassword(checkUser, user.PasswordHash);

                checkUser.PasswordHash = passwordHash;
                checkUser.Token = newToken;

                await _userManage.UpdateAsync(user);
                TempData["success"] = "Password updated successfully.";
                return RedirectToAction("Login", "Account");
            }
            else
            {
                TempData["error"] = "Email not found or token is not right";
                return RedirectToAction("ForgetPass", "Account");
            }
        }

        public async Task<IActionResult> SendMailForgotPass(AppUserModel model)
        {
            var checkMail = await _userManage.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

            if (checkMail == null)
            {
                TempData["error"] = "Email not found";
                return RedirectToAction("ForgetPass", "Account");
            }
            else
            {
                string token = Guid.NewGuid().ToString();
                checkMail.Token = token;

                _dataContext.Users.Update(checkMail);
                await _dataContext.SaveChangesAsync();

                var receiver = checkMail.Email;
                var subject = "Change password for user " + checkMail.Email;

                var callbackUrl = $"{Request.Scheme}://{Request.Host}/Account/NewPass?email={checkMail.Email}&token={token}";

                var message = "Click on link to change password: <a href='" + callbackUrl + "'>Click here</a>";

                await _emailSender.SendEmailAsync(receiver, subject, message);
            }

            TempData["success"] = "An email has been sent to your registered email address with password reset instructions";
            return RedirectToAction("ForgetPass", "Account");
        }

        public async Task<IActionResult> ForgetPass(string returnUrl)
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
        public async Task<IActionResult> History()
        {
            if ((bool)!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            var Orders = await _dataContext.Orders.Where(od => od.UserName == userEmail).OrderByDescending(od => od.Id).ToListAsync();
            ViewBag.UserEmail = userEmail;
            return View(Orders);
                
        }

        public async Task<IActionResult> CancelOrder(string ordercode)
        {
            if ((bool)!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            try
            {
                var order = await _dataContext.Orders.Where(o => o.OrderCode == ordercode).FirstAsync();
                order.Status = 0;
                _dataContext.Orders.Update(order);
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest("An error occured while cancelling the order");
            }
            return RedirectToAction("History", "Account");
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
                    await _userManage.AddToRoleAsync(newUser, "user");
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
