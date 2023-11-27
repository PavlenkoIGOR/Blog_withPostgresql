using Blog.BLL.Models;
using Blog.BLL.ViewModels;
using Blog_withPostgresql.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Security.Claims;

namespace Blog_withPostgresql.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserRepo _userRepo;

        public HomeController(ILogger<HomeController> logger, IUserRepo userRepo)
        {
            _logger = logger;
            _userRepo = userRepo;
        }

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
                _logger.LogInformation($"пользователь аутентифицирован {DateTime.UtcNow}");
                return View("GreetingPage", blogVM);
            }
            else
            {
                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}