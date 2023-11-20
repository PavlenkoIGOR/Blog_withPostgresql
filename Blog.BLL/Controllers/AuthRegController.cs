using Microsoft.AspNetCore.Mvc;
using Blog_withPostgresql.Repositories;
using Blog.BLL.ViewModel;
using Blog.BLL.Models;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace Blog_withPostgresql.Controllers
{
    //[Route("[controller]")]
    //[ApiController]
    public class AuthRegController : Controller
    {
        IUserRepo _userRepo;
        public AuthRegController(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        #region Adduser
        [HttpGet]
        //[Route("RegUser")]
        public IActionResult RegUser()
        {
            return View("RegUser");
        }
        [HttpPost]
        public async Task<IActionResult> RegUser(UserRegViewModel userView)
        {
            if (ModelState.IsValid)
            {
                await _userRepo.AddUser(userView);
                return RedirectToAction("UserBlog", "Blog");
            }
            return View(userView);
        }
        #endregion

        #region Login
        //[Route("AuthUser")]
        [HttpGet]
        public IActionResult AuthUser()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult AuthUser(UserAuthViewModel ubVM)
        {
            if (ModelState.IsValid)
            {
                User userE = _userRepo.GetUserByEmail(ubVM.Email);
                User userP = _userRepo.GetUserByPassword(ubVM.Password);
                if (userP?.Password != ubVM.Password)
                {
                    ViewData["InvalidPassword"] = "Неправильный пароль";
                    return View(ubVM);
                }
                else
                {
                    UserBlogViewModel blogVM = new UserBlogViewModel()
                    {
                        Email = ubVM.Email,
                        UserId = userE.Id,
                        Role = userE.Role,
                        UserAge = userE.Age,
                        UserName = userE.Name
                    };
                    return View("GreetingPage", blogVM);
                }
            }
            return View();
        }
        #endregion

        [HttpGet]
        //[Route("EditUser")]
        public IActionResult EditUser()
        {
            return View();
        }
    }
}



/*                    var claims = new List<Claim>()
                {
                    new Claim(JwtRegisteredClaimNames.Sub, "Arcadiy"),
                    new Claim(JwtRegisteredClaimNames.Email, "Arc@mail.ru")
                };
                    var token = new JwtSecurityToken();*/