using Blog_withPostgresql.Repositories;
using Blog.BLL.ViewModel;
using Blog.BLL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

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
        public async Task<IActionResult> AuthUser(UserAuthViewModel ubVM)
        {
            if (ModelState.IsValid)
            {
                User userE = await _userRepo.GetUserByEmail(ubVM.Email);
                User userP = await _userRepo.GetUserByPassword(PasswordHash.HashPassword(ubVM.Password));

                if ((userE?.Email != ubVM.Email) & (userP?.Password == PasswordHash.HashPassword(ubVM.Password)))
                {
                    ViewData["InvalidPassword"] = "Неправильный пароль";
                    return View(ubVM);
                }
                else if ((userE?.Email == ubVM.Email) & (userP?.Password != PasswordHash.HashPassword(ubVM.Password)))
                {
                    ViewData["InvalidPassword"] = "Неправильный адрес почты";
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
                    var claims = new List<Claim>() 
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, blogVM.UserName),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, blogVM.Role)
                    };

                    // создаем объект ClaimsIdentity
                    ClaimsIdentity id = new ClaimsIdentity
                        (
                        claims,
                        "BlogApplication_Cookie",
                        ClaimsIdentity.DefaultNameClaimType,
                        ClaimsIdentity.DefaultRoleClaimType
                        );
                    // установка аутентификационных куки
                    await HttpContext.SignInAsync("BlogApplication_Cookie", new ClaimsPrincipal(id));

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



/*
 var claims = new List<Claim>()
                {
                    new Claim(JwtRegisteredClaimNames.Sub, "Arcadiy"),
                    new Claim(JwtRegisteredClaimNames.Email, "Arc@mail.ru")
                };
                    var token = new JwtSecurityToken();
*/