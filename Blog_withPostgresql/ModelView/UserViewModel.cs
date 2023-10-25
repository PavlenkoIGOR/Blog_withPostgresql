using System.ComponentModel.DataAnnotations;

namespace Blog_withPostgresql.ModelView
{
    public class UserViewModel
    {
        [DataType(DataType.Text)]
        [Required]
        [Display(Name = "Никнейм", Prompt = "Введите Никнейм")]
        public string Name { get; set; }

        [EmailAddress]
        [Required]
        [Display(Name = "Почта", Prompt = "Введите свой Email")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [Display(Name = "Пароль", Prompt = "Введите пороль")]
        public string Password { get; set; }

    }
}
