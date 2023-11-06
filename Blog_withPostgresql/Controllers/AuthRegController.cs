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
        public IActionResult AuthUser(UserViewModel uVM)
        {
            User user = _userRepo.GetUserByEmail(uVM.Email);
            UserBlogViewModel blogVM = new UserBlogViewModel()
            {
                Email = uVM.Email,
                UserId = user.Id,
                Role = user.Role,
                UserAge = user.Age,
                UserName = user.Name
            };

            return View("UserBlog", blogVM);
        }
        [HttpPost]
        public IActionResult AuthUser()
        {

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
