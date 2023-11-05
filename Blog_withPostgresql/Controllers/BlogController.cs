using Blog_withPostgresql.Models;
using Blog_withPostgresql.ModelView;
using Blog_withPostgresql.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data.Common;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
