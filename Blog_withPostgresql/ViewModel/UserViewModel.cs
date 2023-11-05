using System.ComponentModel.DataAnnotations;

namespace Blog_withPostgresql.ModelView
{
    public class UserViewModel
    {
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name = "Никнейм", Prompt = "Введите Никнейм")]
        public string Name { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name = "Почта", Prompt = "Введите свой Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name = "Возраст", Prompt = "Введите свой возраст")]
        public int Age { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name = "Пароль", Prompt = "Введите пороль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [Display(Name = "Подтвердить пароль", Prompt = "Введите пароль повторно")]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string ConfirmPassword { get; set; }

    }
}
