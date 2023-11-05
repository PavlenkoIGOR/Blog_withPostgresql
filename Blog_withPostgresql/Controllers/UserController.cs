using Blog_withPostgresql.ModelView;
using Blog_withPostgresql.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Blog_withPostgresql.Controllers
{
    public class UserController : Controller
    {
        IUserRepo _userRepo;
        /*private IConfiguration _configuration;*/
        public UserController(/*IConfiguration configuration,*/ IUserRepo userRepo)       
        {
            /*_configuration = configuration;*/
            _userRepo = userRepo;
        }

        [HttpGet]
        public IActionResult AddUser() 
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddUser(UserViewModel userView)
        {
            _userRepo.AddUser(userView);
            return RedirectToAction("AddUser", "User");
        }

        [HttpGet]
        public IActionResult AuthUser()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AuthUser(UserViewModel userView)
        {
            
            return RedirectToAction("UserBlog", "User");
        }
    }
}
