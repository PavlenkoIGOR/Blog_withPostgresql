using Blog.BLL;
using Blog.BLL.ViewModels;
using Blog.Data;
using Blog.Data.Models;
using Blog_withPostgresql.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace Blog_withPostgresql.Controllers
{
    //[ApiController]
    public class HomeController : Controller
    {
        private readonly IMyLogger _logger;
        private readonly IUserRepo _userRepo;
        readonly IWebHostEnvironment _env;
        public HomeController(IMyLogger logger, IUserRepo userRepo, IWebHostEnvironment env)
        {
            _logger = logger;
            _userRepo = userRepo;
            _env = env;
        }

        [HttpGet]
        //[Route("Index")]
        public IActionResult Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var currentUser = HttpContext.User;
                var userId = currentUser.FindFirstValue(ClaimTypes.NameIdentifier); //представляет идентификатор пользователя.

                User user = _userRepo.GetUserByEmail(currentUser.Identity.Name);
                UserBlogViewModel blogVM = new UserBlogViewModel()
                {
                    Email = user.Email,
                    UserId = user.Id,
                    Role = user.Role,
                    UserAge = user.Age,
                    UserName = user.Name
                };
                _logger.WriteEvent($"Пользователь {blogVM.UserName} аутентифицирован {DateTime.UtcNow}");
                WriteAction.CreateLogFolder_File(_env, "auth", $"Пользователь {blogVM.UserName} аутентифицирован.");
                return RedirectToAction("GreetingPage", blogVM);
            }
            else
            {
                return View();
            }
        }

        //[Route("GreetingPage")]
        [HttpGet]
        public IActionResult GreetingPage(UserBlogViewModel ubVM)
        {
            return View(ubVM);
        }

        //[Route("Privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        //[Route("Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}