using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.Cookies;
using Blog.BLL.ViewModels;
using Microsoft.AspNetCore.Http;
using Blog_withPostgresql.Repositories;
using Blog.Data.Models;

namespace Blog_withPostgresql.Controllers
{
    //[Route("[controller]")]
    //[ApiController]
    public class AuthRegController : Controller
    {
        IUserRepo _userRepo;
        private readonly ILogger<AuthRegController> _logger;


        public AuthRegController(IUserRepo userRepo, ILogger<AuthRegController> logger)
        {
            _userRepo = userRepo;
            _logger = logger;
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
            User user = new User() 
            {
                    //Id
                Name = userView.Name,
                Age = userView.Age,
                Email = userView.Email,
                Password = PasswordHash.HashPassword(userView.Password)
            };
            if (ModelState.IsValid) //необходимо добавить проверку наличия БД и её заполнености
            {
                int userId = await _userRepo.AddUserAndGetId(user);
                await Authenticate(userView.Email);
                user = _userRepo.GetUserById(userId);

                UserBlogViewModel usersBlogModel = new UserBlogViewModel() 
                {
                    UserName = userView.Name,
                    Role = user.Role
                };
                //return RedirectToAction("UserBlog", "Posts");
                return RedirectToAction("GreetingPage", "Home", usersBlogModel);
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
                User userE = _userRepo.GetUserByEmail(ubVM.Email);
                User userP = _userRepo.GetUserByPassword(PasswordHash.HashPassword(ubVM.Password));

                if ((userE?.Email != ubVM.Email) & (userP?.Password == PasswordHash.HashPassword(ubVM.Password)))
                {
                    ViewData["InvalidPassword"] = "Неправильный адрес почты";
                    return View(ubVM);
                }
                if ((userE?.Email == ubVM.Email) & (userP?.Password != PasswordHash.HashPassword(ubVM.Password)))
                {
                    ViewData["InvalidPassword"] = "Неправильный пароль";
                    return View(ubVM);
                }
                if ((userE?.Email != ubVM.Email) & (userP?.Password != PasswordHash.HashPassword(ubVM.Password)))
                {
                    ViewData["InvalidPassword"] = "Неправильный пароль и/или логин";
                    return View(ubVM);
                }
                    UserBlogViewModel blogVM = new UserBlogViewModel()
                    {
                        Email = ubVM.Email,
                        UserId = userE.Id,
                        Role = userE.Role,
                        UserAge = userE.Age,
                        UserName = userE.Name
                    };
                    await Authenticate(userE.Email); // аутентификация

                    //var claims = new List<Claim>()
                    //{
                    //    new Claim(ClaimsIdentity.DefaultNameClaimType, blogVM.UserName),
                    //    new Claim(ClaimsIdentity.DefaultRoleClaimType, blogVM.Role)
                    //};
                    //// создаем объект ClaimsIdentity
                    //ClaimsIdentity claimId = new ClaimsIdentity
                    //    (
                    //    claims,
                    //    "BlogApplication_Cookie",
                    //    ClaimsIdentity.DefaultNameClaimType,
                    //    ClaimsIdentity.DefaultRoleClaimType
                    //    );
                    ////_logger.LogInformation("Nen fdsfds {@user}", blogVM);
                    ////var cl = GenerationTokenStation(blogVM.UserId);
                    //// установка аутентификационных куки
                    //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimId));
                    ////await HttpContext.SignInAsync("BlogApplication_Cookie", new ClaimsPrincipal(id));
                    return RedirectToAction("GreetingPage", "Home", blogVM);
                
            }
            else
            {
                ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }




        public string GenerationTokenStation(int userId)
        {
            var key = "Тут ключ для подписи";

            var claims = new List<Claim> { new Claim("UserId", userId.ToString()) };
            var jwt = new JwtSecurityToken(
                    issuer: "Кто выдал токен",
                    audience: "Для кого выдан",
                    claims: claims,
                    expires: DateTime.UtcNow.AddSeconds(60),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)), SecurityAlgorithms.HmacSha256));

            var tokenHandler = new JwtSecurityTokenHandler().WriteToken(jwt);
            return tokenHandler.ToString();
        }
        #endregion

        //[Authorize]
        [HttpGet]
        //[Route("EditUser")]
        public IActionResult EditUser()
        {
            return View();
        }






        private async Task Authenticate(string userMail)
        {
            User user = _userRepo.GetUserByEmail(userMail);
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "BlogApplication_Cookie", ClaimTypes.Name, ClaimTypes.Role);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
