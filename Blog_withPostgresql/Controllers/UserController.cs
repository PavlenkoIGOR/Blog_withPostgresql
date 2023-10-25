using Blog_withPostgresql.Models;
using Blog_withPostgresql.ModelView;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data.Common;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Blog_withPostgresql.Controllers
{
    public class UserController : Controller
    {
        private IConfiguration _configuration;
        public UserController(IConfiguration configuration)       
        {
            _configuration = configuration;
        }
        [HttpGet]
        public IActionResult AddUser() 
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddUser(UserViewModel userView)
        {
            string connectionString = /*"Server=localhost;Username=postgres;Port=5432;Database=Bethlam;UserId=postgres;Password=post1234;";*/_configuration.GetConnectionString("Bethlam");
            string sqlExpression = $"insert into users (name, email, password) values ({userView.Name}, {userView.Email}, {userView.Password})";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new NpgsqlCommand(sqlExpression, connection)) /************************************ так НЕ работает **************************/
                {
                    command.ExecuteNonQuery();
                }

                /*****************так работает**************************/

                //using (var command = connection.CreateCommand())
                //{
                //    command.CommandText = "INSERT INTO Users (Name, Email, Password) VALUES (@Name, @Email, @Password)";
                //    command.CommandText = "INSERT INTO Users (Name, Email, Password) VALUES (@Name, @Email, @Password)";
                //    command.Parameters.AddWithValue("@Name", userView.Name);
                //    command.Parameters.AddWithValue("@Email", userView.Email);
                //    command.Parameters.AddWithValue("@Password", userView.Password);
                //    command.ExecuteNonQuery();
                //}
            }
            return RedirectToAction("AddUser", "User");
        }
    }
}
