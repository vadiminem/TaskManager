using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models.Authorization
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Некорректный email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Некорректный пароль")]
        public string Password { get; set; }
    }
}
