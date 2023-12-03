using Blog.BLL.Models;
using Blog.BLL.Repositories;
using Blog.BLL.ViewModel;
using Blog.BLL.ViewModels;
using Blog_withPostgresql.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace Blog_withPostgresql.Controllers
{
    public class BlogController : Controller
    {
        IUserRepo _userRepo;
        IPostRepo _postRepo;
        IConfiguration _configuration;
        ITegRepo _tegRepo;
        public BlogController(IConfiguration configuration, IUserRepo userRepo, IPostRepo postRepo, ITegRepo tegRepo)       
        {
            _configuration = configuration;
            _userRepo = userRepo;
            _postRepo = postRepo;
            _tegRepo = tegRepo;
        }
        
        #region ShowAllPosts
        [HttpGet]
        public async Task<IActionResult> AllPostsPage()
        {
            List<Teg> tegsModel = await _tegRepo.GetAllTegs();


            List<Post> posts = await _postRepo.GetAllPosts();
            List<AllPostsViewModel> allpVM = posts.Select(p => new AllPostsViewModel
            {
                Id = p.Id,
                PublicationTime = p.PublicationDate,
                Title = p.postTitle,
                Text = p.postText,
                Author = _userRepo.GetUserById(p.UserId).Name
                //TegsList = tegForView
            }).ToList();
            ViewBag.List = tegsModel;
            //await _logger.WriteEvent("Переход на страницу показа всех пользователей");
            return View("AllPostsPage", allpVM);
        }

        [HttpPost]
        public IActionResult AllPostsPage(string selectedRole)
        {
  
            return View();
        }
        #endregion

        #region ShowUsers Настроено!

        [Authorize(Roles = "Administrator")] //так авторизация работает только для пользователей у которых  Role == "admin". На данный момент настроено через проверку в View Index
        [HttpGet]
        public async Task<IActionResult> ShowUsers()
        {
            //bool role = HttpContext.User.IsInRole("administrator");//на всякий
            if (User.IsInRole("administrator") | User.IsInRole("Administrator"))
            {
                List<User> users1 = await _userRepo.GetAllUsers();
                List<UsersViewModel> users = users1.Select(u => new UsersViewModel()
                {
                    Id = u.Id,
                    Name = u.Name,
                    Age = u.Age,
                    Email = u.Email,
                    RoleType = u.Role
                }).ToList();
                return View(users);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult ShowUsers(string selectedRole)
        {
            return View();
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> DeleteUser(UsersViewModel usersVM)
        {
            
            await _userRepo.DeleteUser(usersVM.Id);

            //List<User> users1 = await _userRepo.GetAllUsers();
            //List<UsersViewModel> users = users1.Select(u => new UsersViewModel()
            //{
            //    Id = u.Id,
            //    Name = u.Name,
            //    Age = u.Age,
            //    Email = u.Email,
            //    RoleType = u.Role
            //}).ToList();

            return RedirectToAction("ShowUsers", "Blog");
        }
    }
}
