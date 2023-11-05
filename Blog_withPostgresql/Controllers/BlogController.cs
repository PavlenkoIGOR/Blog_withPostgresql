using Blog.BLL.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Blog_withPostgresql.Controllers
{
    public class BlogController : Controller
    {
        private IConfiguration _configuration;
        public BlogController(IConfiguration configuration)       
        {
            _configuration = configuration;
        }
        [HttpGet]
        public IActionResult UserBlog() 
        {
            return View();
        }
        [HttpPost]
        public IActionResult UserBlog(UserBlogViewModel ubVM)
        {

            return View(ubVM);
        }
    }
}
