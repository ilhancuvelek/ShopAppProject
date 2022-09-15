using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Identity;
using ShopApp.Models;
using System.Threading.Tasks;

namespace ShopApp.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerModel);
            }
            var user = new User()
            {
                UserName = registerModel.UserName,
                Email = registerModel.Email,
                LastName = registerModel.LastName,
                FirstName = registerModel.FirstName
            };
            var result= await _userManager.CreateAsync(user, registerModel.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Login", "Account");
            }
            ModelState.AddModelError("", "Bilinmeyen bir hata oluştu.Lütfen tekrar deneyiniz.");
            return View(registerModel);
        }
    }
}
