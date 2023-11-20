using Microsoft.AspNetCore.Mvc;
using Blog_withPostgresql.Repositories;
using Blog.BLL.ViewModel;
using Blog.BLL.Models;

namespace Blog_withPostgresql.Controllers
{
    public class AuthRegController : Controller
    {
        IUserRepo _userRepo;
        public AuthRegController(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }
        #region Adduser
        [HttpGet]
        public IActionResult AddUser()
        {
            return View("RegUser");
        }
        [HttpPost]
        public async Task<IActionResult> AddUser(UserViewModel userView)
        {
            _userRepo.AddUser(userView);
            return RedirectToAction("UserBlog", "Blog");
        }
        #endregion

        #region Login
        [HttpGet]
        public IActionResult AuthUser()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AuthUser(UserBlogViewModel ubVM)
        {
            User user = _userRepo.GetUserByEmail(ubVM.Email);
            UserBlogViewModel blogVM = new UserBlogViewModel()
            {
                Email = ubVM.Email,
                UserId = user.Id,
                Role = user.Role,
                UserAge = user.Age,
                UserName = user.Name
            };
            return RedirectToAction("UserBlog", "Blog");
        }
        #endregion

        [HttpGet]
        public IActionResult EditUser()
        {
            return View();
        }
    }
}
