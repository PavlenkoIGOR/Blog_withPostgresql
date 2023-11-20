using Blog.BLL.ViewModel;
using Blog_withPostgresql.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Blog_withPostgresql.Controllers
{
    public class BlogController : Controller
    {
        IUserRepo _userRepo;
        IConfiguration _configuration;
        public BlogController(IConfiguration configuration, IUserRepo userRepo)       
        {
            _configuration = configuration;
            _userRepo = userRepo;
        }
        [HttpGet]
        public IActionResult UserBlog(UserBlogViewModel ubVM) 
        {            
            return View(ubVM);
        }
        [HttpPost]
        public IActionResult UserBlog()
        {

            return View();
        }
    }
}
