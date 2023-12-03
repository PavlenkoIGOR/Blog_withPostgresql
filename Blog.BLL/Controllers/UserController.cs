using Blog.BLL.Models;
using Blog.BLL.ViewModels;
using Blog_withPostgresql.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Blog.BLL.Controllers
{
    public class UserController : Controller
    {
        private IUserRepo _userRepo;

        public UserController(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }
        [HttpGet]
        public async Task<IActionResult> EditUserPage(int id)
        {
            User user = _userRepo.GetUserById(id);


            if (user == null)
            {
                return StatusCode(404);
            }
            UsersViewModel viewModel = new UsersViewModel();
            viewModel.Id = id;
            viewModel.Email = user.Email;
            viewModel.Name = user.Name;
            viewModel.Age = user.Age;
            viewModel.RoleType = user.Role;
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserByAdmin(UsersViewModel usersVM)
        {
            string userRole = String.Empty;
            foreach (RolesViewModel role in Enum.GetValues(typeof(RolesViewModel)))
            {
                if (role.GetHashCode() == Convert.ToInt32(usersVM.RoleType))
                {
                    // Найдено соответствующее значение по хэш-коду
                    Console.WriteLine("Найдено значение: " + role.ToString());
                    userRole = role.ToString();
                }
            }
            if (ModelState.IsValid)
            {
                UsersViewModel uVM = new UsersViewModel();
                User user = _userRepo.GetUserById(usersVM.Id);
                if (user == null)
                {
                    return BadRequest("Пользователь не найден!");
                }
                else
                {
                    user.Id = usersVM.Id;
                    uVM.Id = usersVM.Id;
                    uVM.Email = user.Email;
                    uVM.Name = user.Name;
                    uVM.Age = usersVM.Age;
                    uVM.RoleType = userRole;

                    user.Age = usersVM.Age;
                    user.Id = usersVM.Id;
                    user.Email = usersVM.Email;
                    user.Name = usersVM.Name;

                    await _userRepo.EditUser(user);

                }

                return View("EditUserPage", uVM);
            }
            return BadRequest("Пользователь не найден!");
        }
    }
}
