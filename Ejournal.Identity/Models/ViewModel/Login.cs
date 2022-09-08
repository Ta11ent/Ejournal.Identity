using System.ComponentModel.DataAnnotations;

namespace Ejournal.Identity.Models
{
    public class Login
    {
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool Remeber { get; set; }
        public string ReturnUrl { get; set; }
        public bool Invalid { get; set; } 
    }
}
