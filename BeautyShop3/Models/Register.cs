using System.ComponentModel.DataAnnotations;

namespace BeautyShop3.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Имя пользователя обязательно.")]
        [StringLength(100, ErrorMessage = "Имя должно быть не длиннее 100 символов.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Телефон обязателен.")]
        [RegularExpression(@"^(\+7|8)?\s?\(?\d{3}\)?[\s-]?\d{3}[\s-]?\d{2}[\s-]?\d{2}$",
            ErrorMessage = "Введите корректный номер телефона в формате +7 XXX XXX-XX-XX или 8 XXX XXX-XX-XX.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Пароль обязателен.")]
        [MinLength(6, ErrorMessage = "Пароль должен быть не менее 6 символов.")]
        public string Password { get; set; }

    }
}
