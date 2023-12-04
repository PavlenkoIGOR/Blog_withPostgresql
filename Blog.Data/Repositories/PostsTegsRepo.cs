using Blog.Data.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Blog.Data.Repositories;

public interface IPostsTegsRepo
{
    public Task InsertIntoPostTegs(PostsTegs postsTegs);
}

public class PostsTegsRepo : IPostsTegsRepo
{
    public static IConfiguration _configuration;
    public PostsTegsRepo(IConfiguration configuration)
    {
        _configuration = configuration;
}
    public async Task InsertIntoPostTegs (PostsTegs postsTegs)
    {
        string connectionString = _configuration.GetConnectionString("Bethlem"); /* = "Server=localhost;Username=postgres;Port=5432;Database=Bethlem;UserId=postgres;Password=postg1234;";*/
        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            await connection.OpenAsync();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO posts_tegs (post_id, teg_id) VALUES (@post_id, @teg_id)";
                command.Parameters.AddWithValue("@post_id", postsTegs.PostId);
                command.Parameters.AddWithValue("@teg_id", postsTegs.TegId);
                await command.ExecuteNonQueryAsync();
            }
            connection.CloseAsync().Wait();
        }
    }
}
