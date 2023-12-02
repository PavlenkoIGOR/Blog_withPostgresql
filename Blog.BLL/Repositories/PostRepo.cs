using Blog.BLL.Models;

using Blog.BLL.ViewModels;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Blog_withPostgresql.Repositories
{
    public interface IPostRepo
    {
        public Task<int> AddPost(Post post);
        public User GetUserByEmail(string email);
        public User GetUserByPassword(string password);
        public Post GetPostById(int id);
        public Task EditPost(UsersViewModel usersViewModel);
        public Task<List<Post>> GetAllPosts();
    }


    public class PostRepo : IPostRepo
    {
        public static IConfiguration _configuration;
        string connectionString = _configuration.GetConnectionString("Bethlem"); /* = "Server=localhost;Username=postgres;Port=5432;Database=Bethlem;UserId=postgres;Password=postg1234;";*/
        public PostRepo(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> AddPost(Post post)
        {
            int postId = default;
            string connectionString = _configuration.GetConnectionString("Bethlem");
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO posts (post_title, post_text, publication_date, user_id) VALUES (@post_title, @post_text, @publication_date, @user_id) RETURNING id";
                    command.Parameters.AddWithValue("@post_title", post.postTitle);
                    command.Parameters.AddWithValue("@post_text", post.postText);
                    command.Parameters.AddWithValue("@user_id", post.UserId);
                    command.Parameters.AddWithValue("@publication_date", post.PublicationDate);
                    //await command.ExecuteNonQueryAsync();
                    postId = (int)(await command.ExecuteScalarAsync()); // Получаем id нового поста
                }
                connection.CloseAsync().Wait();
            }
            return postId;
        }

        public User GetUserByEmail(string email)
        {
            string connectionString = _configuration.GetConnectionString("Bethlem"); /* = "Server=localhost;Username=postgres;Port=5432;Database=Bethlem;UserId=postgres;Password=postg1234;";*/
            User user = new User();
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                string selectUserByEmail = "select * from users where email = @email";
                using (NpgsqlCommand command = new NpgsqlCommand(selectUserByEmail, connection))
                {
                    command.Parameters.AddWithValue("@email", email);
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user.Id = reader.GetInt32(0);//id
                            user.Name = reader.GetString(1);//Name
                            user.Age = reader.GetInt32(2);//Age                            
                            user.Email = reader.GetString(3);//Password
                            user.Password = reader.GetString(4);//Role
                            user.Role = reader.GetString(5);//Password
                        }
                        reader.Close();
                    }
                }
                connection.Close();
            }
            return user;
        }
        public User GetUserByPassword(string password)
        {
            string connectionString = _configuration.GetConnectionString("Bethlem"); /* = "Server=localhost;Username=postgres;Port=5432;Database=Bethlem;UserId=postgres;Password=postg1234;";*/
            User user = new User();
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                string selectUserByEmail = "select * from users where password = @password";
                using (NpgsqlCommand command = new NpgsqlCommand(selectUserByEmail, connection))
                {
                    command.Parameters.AddWithValue("@password", password);
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user.Id = reader.GetInt32(0);//id
                            user.Name = reader.GetString(1);//Name
                            user.Age = reader.GetInt32(2);//Age
                            user.Role = reader.GetString(5);//Role
                            user.Email = reader.GetString(3);//Email
                            user.Password = reader.GetString(4);//Password                            
                        }
                        reader.Close();
                    }
                }
                connection.Close();
            }
            return user;
        }
        public Post GetPostById(int id)
        {
            string connectionString = _configuration.GetConnectionString("Bethlem"); /* = "Server=localhost;Username=postgres;Port=5432;Database=Bethlem;UserId=postgres;Password=postg1234;";*/
            Post post = new Post();
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                string selectUserByEmail = "select * from posts where id = @id";
                using (NpgsqlCommand command = new NpgsqlCommand(selectUserByEmail, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            post.Id = reader.GetInt32(0);//id
                            post.postTitle = reader.GetString(1);//post_title
                            post.postText = reader.GetString(2);//post_text
                            post.PublicationDate = reader.GetDateTime(3);//PublicationDate
                            post.UserId = reader.GetInt32(4);//UserId
                        }
                        reader.Close();
                    }
                }
                connection.Close();
            }
            return post;
        }

        public async Task<List<Post>> GetAllPosts()
        {
            List<Post> posts = new List<Post>();

            string connectionString = _configuration.GetConnectionString("Bethlem");

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();
                string selectUserByEmail = "select * from posts";
                using (NpgsqlCommand command = new NpgsqlCommand(selectUserByEmail, connection))
                {
                    using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Post post = new Post();
                            post.Id = reader.GetInt32(0);//id
                            post.postTitle = reader.GetString(1);//postTitle
                            post.postText = reader.GetString(2);//postText
                            post.PublicationDate = reader.GetDateTime(3);//PublicationDate
                            post.UserId = reader.GetInt32(4);//UserId
                            posts.Add(post);
                        }
                    }
                }
                connection.CloseAsync().Wait();
            }
            return posts;
        }


        public async Task EditPost(UsersViewModel usersVM)
        {
            string connectionString = _configuration.GetConnectionString("Bethlem"); /* = "Server=localhost;Username=postgres;Port=5432;Database=Bethlem;UserId=postgres;Password=postg1234;";*/
            User user = new User();
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                /*обновление данных*/
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "update users set name = @name, age = @age, email = @email, role = @role where id = @id";
                    command.Parameters.AddWithValue("@name", usersVM.Name);
                    command.Parameters.AddWithValue("@email", usersVM.Email);
                    command.Parameters.AddWithValue("@age", usersVM.Age);
                    command.Parameters.AddWithValue("@role", usersVM.RoleType);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
    }
}
