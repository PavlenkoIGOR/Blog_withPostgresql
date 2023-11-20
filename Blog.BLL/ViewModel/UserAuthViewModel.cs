using System.ComponentModel.DataAnnotations;

namespace Blog.BLL.ViewModel
{
    public class UserAuthViewModel
    {
        [EmailAddress]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name = "Почта", Prompt = "Введите свой Email")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name = "Пароль", Prompt = "Введите пороль")]
        public string Password { get; set; }
    }
}
