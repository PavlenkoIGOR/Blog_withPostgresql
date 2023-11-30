using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.BLL.ViewModels
{
    public enum RolesViewModel
    {
        [Display(Name = "User")]
        User,

        [Display(Name = "Moderator")]
        Moderator,

        [Display(Name = "Administrator")]
        Administrator
    }
}
