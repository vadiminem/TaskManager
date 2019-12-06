using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models.Authorization
{
    public class RegistrationModel
    {
        [Required(ErrorMessage = "Не указан Email")]
        public string Email { get; set; }

        public string Username { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароль введен неверно")]
        public string ConfirmPassword { get; set; }
    }
}
