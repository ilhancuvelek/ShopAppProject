using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShopApp.EmailServices;
using ShopApp.Identity;
using ShopApp.Models;
using System.Threading.Tasks;

namespace ShopApp.Controllers
{
    [AutoValidateAntiforgeryToken] //csrf token bu sadece post lara uygular.
    public class AccountController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IEmailSender _emailSender;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        public  IActionResult Login(string ReturnUrl=null)
        {
            //örneğin giriş yapmadan adminproducts a tıkladım giriş yapılmadığından login e yönlendirir.
            //loginden giriş yapıldığında adminproduct a yönlenidirir ReturnUrl sayesinde.
            return View(new LoginModel() { ReturnUrl=ReturnUrl});
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginModel);
            }
            var user= await _userManager.FindByEmailAsync(loginModel.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Bu email ile daha önce hesap açılmamış.");
                return View(loginModel);
            }
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("", "Lütfen email hasabınıza gelen link ile hesabınızı onaylayın.");
                return View(loginModel);
            }
            var result = await _signInManager.PasswordSignInAsync(user, loginModel.Password, true, false);
            if (result.Succeeded)
            {
                return Redirect(loginModel.ReturnUrl ?? "~/");//input-hidden ile returnurl yi yaptığımız için loginModel de kullanabiliyoruz burda
            }
            ModelState.AddModelError("", "Girilen email veya parola yanlış.");
            return View(loginModel);
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerModel);
            }

            var user = new User()
            {
                FirstName = registerModel.FirstName,
                LastName = registerModel.LastName,
                UserName = registerModel.UserName,
                Email = registerModel.Email
            };

            var result = await _userManager.CreateAsync(user, registerModel.Password);
            if (result.Succeeded)
            {
                // generate token
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var url = Url.Action("ConfirmEmail", "Account", new
                {
                    userId = user.Id,
                    token = code
                });

                // email
                await _emailSender.SendEmailAsync(registerModel.Email, "Hesabınızı onaylayınız.", $"Lütfen email hesabınızı onaylamak için linke <a href='https://localhost:5001{url}'>tıklayınız.</a>");
                return RedirectToAction("Login", "Account");
            }

            ModelState.AddModelError("", "Bilinmeyen hata oldu lütfen tekrar deneyiniz.");
            return View(registerModel);

        }
        public async Task<IActionResult> ConfirmEmail(string userId, string token)//register dan alacak
        {
            if (userId == null || token == null)
            {
                CreateMessage("Geçersiz token", "danger");
                return View();
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    CreateMessage("Hesabınız Onaylandı", "success");
                    return View();
                }
            }
            CreateMessage("Hesabınız Onaylanmadı", "warning");
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("~/");
        }

        //şifremi unuttum
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
            if (string.IsNullOrEmpty(Email))
            {
                return View();
            }
            var user = await _userManager.FindByEmailAsync(Email);
            if (user==null)
            {
                return View();
            }
            // generate token
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var url = Url.Action("ResetPassword", "Account", new
            {
                userId = user.Id,
                token = code
            });
            // email
            await _emailSender.SendEmailAsync(Email, "Reset Password", $"Parolanızı Yenilemek için linke <a href='https://localhost:5001{url}'>tıklayınız.</a>");
            return View();
        }
        public IActionResult ResetPassword(string userId, string token)
        {
            if (userId==null||token==null)
            {
                return RedirectToAction("Index", "Home");
            }
            var model = new ResetPasswordModel()
            {
                Token = token
            };
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            if (!ModelState.IsValid)
            {
                return View(resetPasswordModel);
            }
            var user=await _userManager.FindByEmailAsync(resetPasswordModel.Email);
            if (user==null)
            {
                return RedirectToAction("Index", "Home");
            }
            var result = await _userManager.ResetPasswordAsync(user, resetPasswordModel.Token, resetPasswordModel.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Login", "Account");
            }
            return View(resetPasswordModel);
        }
        //şifremi unuttum --Son--

       

        private void CreateMessage(string message, string alerttype)
        {
            var msg = new AlertMessage
            {
                Message = message,
                AlertType = alerttype
            };
            TempData["message"] = JsonConvert.SerializeObject(msg);
        }
    }
}
