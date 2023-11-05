using Microsoft.AspNetCore.Mvc;
using Blog_withPostgresql.Repositories;
using Blog.BLL.ViewModel;

namespace Blog_withPostgresql.Controllers
{
    public class UserController : Controller
    {
        IUserRepo _userRepo;
        public UserController(IUserRepo userRepo)       
        {
            _userRepo = userRepo;
        }
        #region Adduser
        [HttpGet]
        public IActionResult AddUser() 
        {
            return View();
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
        public IActionResult AuthUser(UserViewModel userView)
        {
            
            return RedirectToAction("UserBlog", "Blog");
        }
        #endregion
    }
}
