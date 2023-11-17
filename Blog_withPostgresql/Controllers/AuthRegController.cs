using Microsoft.AspNetCore.Mvc;
using Blog_withPostgresql.Repositories;
using Blog.BLL.ViewModel;
using Blog.BLL.Models;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace Blog_withPostgresql.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthRegController : Controller
    {
        IUserRepo _userRepo;
        public AuthRegController(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        #region Adduser
        [HttpGet]
        [Route("AddUser")]
        public IActionResult AddUser()
        {
            return View("RegUser");
        }
        [HttpPost]
        public async Task<IActionResult> AddUser(UserViewModel userView)
        {
            await _userRepo.AddUser(userView);
            return RedirectToAction("UserBlog", "Blog");
        }
        #endregion

        #region Login
        [Route("AuthUser")]
        [HttpGet]
        public IActionResult AuthUser()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AuthUser(UserBlogViewModel ubVM)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, "Arcadiy"),
                new Claim(JwtRegisteredClaimNames.Email, "Arc@mail.ru")
            };
            var token = new JwtSecurityToken();

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
        [Route("EditUser")]
        public IActionResult EditUser()
        {
            return View();
        }
    }
}
