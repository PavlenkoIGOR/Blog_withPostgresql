using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.BLL.ViewModels;

public class UsersViewModel
{
    public int Id { get; set; }
    [Display(Name = "Имя пользователя")]
    public string Name { get; set; }
    [Display(Name = "Возраст пользователя")]
    public int Age { get; set; }
    [Display(Name = "Почта пользователя")]
    public string Email { get; set; }
    [Display(Name = "Роль пользователя")]
    public string RoleType { get; set; }
}
