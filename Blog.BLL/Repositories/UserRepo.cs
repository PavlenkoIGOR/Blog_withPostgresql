using Blog.BLL.ViewModel;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Blog_withPostgresql.Repositories
{
    public interface IUserRepo
    {
        public void AddUser(UserViewModel userView);
    }







    public class UserRepo : IUserRepo
    {
        public static IConfiguration _configuration;        
        public UserRepo(IConfiguration configuration)
        {
            _configuration = configuration;
        }





        public async void AddUser(UserViewModel userView)
        {
            string connectionString = _configuration.GetConnectionString("Bethlam"); /* = "Server=localhost;Username=postgres;Port=5432;Database=Bethlam;UserId=postgres;Password=post1234;";*/
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                //using (var command = new NpgsqlCommand(sqlExpression, connection)) /************************************ так НЕ работает **************************/
                //{
                //    command.ExecuteNonQuery();
                //}
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Users (name, age, email, password) VALUES (@name, @age, @email, @password)";
                    command.Parameters.AddWithValue("@name", userView.Name);
                    command.Parameters.AddWithValue("@email", userView.Email);
                    command.Parameters.AddWithValue("@password", PasswordHash.HashPassword(userView.Password));
                    command.Parameters.AddWithValue("@age", userView.Age);
                    command.ExecuteNonQuery();
                }
                connection.CloseAsync().Wait();
            }
        }
    }
}
