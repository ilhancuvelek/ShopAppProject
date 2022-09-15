using System.ComponentModel.DataAnnotations;

namespace ShopApp.Models
{
    public class LoginModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
        //örneğin giriş yapmadan adminproducts a tıkladım giriş yapılmadığından login e yönlendirir.
        //loginden giriş yapıldığında adminproduct a yönlenidirir ReturnUrl sayesinde.
    }
}
