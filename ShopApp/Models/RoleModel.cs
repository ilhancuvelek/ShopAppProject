using System.ComponentModel.DataAnnotations;

namespace ShopApp.Models
{
    public class RoleModel
    {
        [Required]
        public string Name { get; set; }
    }
}
