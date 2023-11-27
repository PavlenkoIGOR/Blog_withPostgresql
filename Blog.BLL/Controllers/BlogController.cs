using Blog.BLL.Models;
using Blog.BLL.ViewModel;
using Blog.BLL.ViewModels;
using Blog_withPostgresql.Repositories;
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
        public BlogController(IConfiguration configuration, IUserRepo userRepo, IPostRepo postRepo)       
        {
            _configuration = configuration;
            _userRepo = userRepo;
            _postRepo = postRepo;
        }
        
        #region ShowAllPosts
        [HttpGet]
        public async Task<IActionResult> AllPostsPage()
        {
            //HashSet<Teg> tegsModel = _context.Tegs.ToHashSet();
            HashSet<string> tegForView = new HashSet<string>();
            //foreach (var tegsItem in tegsModel)
            //{
            //    tegForView.Add(tegsItem.TegTitle);
            //}

            //var post = await _context.Posts.Include(t => t.Tegs).Select(p => new AllPostsViewModel
            //{
            //    Id = p.Id,
            //    Author = p.User.UserName,
            //    PublicationTime = p.PublicationDate,
            //    Title = p.Title,
            //    Text = p.Text,
            //    TegsList = tegForView
            //}).ToListAsync();
            ViewBag.List = tegForView;
            //await _logger.WriteEvent("Переход на страницу показа всех пользователей");
            return View("AllPostsPage"/*, post*/);
        }

        [HttpPost]
        public IActionResult AllPostsPage(string selectedRole)
        {
  
            return View();
        }
        #endregion

        #region ShowUsers Настроено!

        //[Authorize(Roles = "Administrator")] //так авторизация работает только для пользователей у которых  Role == "admin". На данный момент настроено через проверку в View Index
        [HttpGet]
        public async Task<IActionResult> ShowUsers()
        {
            bool role = HttpContext.User.IsInRole("administrator");
            if (HttpContext.User.IsInRole("administrator"))
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
    }
}
