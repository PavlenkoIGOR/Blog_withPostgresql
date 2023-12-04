using System.ComponentModel.DataAnnotations;

namespace Blog.BLL.ViewModels;

public enum RolesViewModel
{
    [Display(Name = "User")]
    User,

    [Display(Name = "Moderator")]
    Moderator,

    [Display(Name = "Administrator")]
    Administrator
}
