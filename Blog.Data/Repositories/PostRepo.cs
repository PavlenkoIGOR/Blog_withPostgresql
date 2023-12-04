using Blog.Data.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Blog_withPostgresql.Repositories;

public interface IPostRepo
{
    public Task<int> AddPost(Post post);
    public Post GetPostById(int id);
    public Task EditPost(Post usersViewModel);
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

    
    public Post GetPostById(int id)
    {
        string connectionString = _configuration.GetConnectionString("Bethlem"); /* = "Server=localhost;Username=postgres;Port=5432;Database=Bethlem;UserId=postgres;Password=postg1234;";*/
        Post post = new Post();
        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            string selectUserByEmail = "select * from posts where id = @id";
            using (NpgsqlCommand command = new (selectUserByEmail, connection))
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
        List<Post> posts = new();

        string connectionString = _configuration.GetConnectionString("Bethlem");

        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            await connection.OpenAsync();
            string selectUserByEmail = "select * from posts";
            using (NpgsqlCommand command = new (selectUserByEmail, connection))
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


    public async Task EditPost(Post post)
    {
        string connectionString = _configuration.GetConnectionString("Bethlem"); /* = "Server=localhost;Username=postgres;Port=5432;Database=Bethlem;UserId=postgres;Password=postg1234;";*/
        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            /*обновление данных*/
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "update posts set post_title = @post_title, post_text = @post_text, publication_date = @publication_date, user_id = @user_id where user_id = @user_id";
                command.Parameters.AddWithValue("@post_title", post.postTitle);
                command.Parameters.AddWithValue("@post_text", post.postText);
                command.Parameters.AddWithValue("@publication_date", post.PublicationDate);
                command.Parameters.AddWithValue("@user_id", post.UserId);
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
    }
}
