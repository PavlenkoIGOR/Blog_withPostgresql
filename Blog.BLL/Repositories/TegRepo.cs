using Blog.BLL.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Blog.BLL.Repositories
{
    public interface ITegRepo
    {
        public Task<int> AddTeg(Teg teg);
        public Teg FindTegByTitle(string tegTitle);
    }

    public class TegRepo : ITegRepo
    {
        public static IConfiguration _configuration;
        public TegRepo(IConfiguration configuration) { _configuration = configuration; }
        public async Task<int> AddTeg(Teg teg)
        {
            int tegId = default;
            string connectionString = _configuration.GetConnectionString("Bethlem"); /* = "Server=localhost;Username=postgres;Port=5432;Database=Bethlem;UserId=postgres;Password=postg1234;";*/
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO tegs (teg_title) VALUES (@teg_title) RETURNING id";
                    command.Parameters.AddWithValue("@teg_title", teg.tegTitle);
                    tegId = (int)(await command.ExecuteScalarAsync()); // Получаем id нового тега
                    //var reader = await command.ExecuteReaderAsync();
                    //если получить весь список тегов
                    //while (reader.Read())
                    //{
                    //    tegId = (int)reader["id"]; // Получаем id новых тегов
                    //                                   // Теперь, можно использовать tegId, чтобы выполнить другие операции
                    //}
                    //await command.ExecuteNonQueryAsync();
                }
                connection.CloseAsync().Wait();
            }
            return tegId;
        }

        public Teg FindTegByTitle(string tegTitle)
        {
            Teg teg = new Teg();

            string connectionString = _configuration.GetConnectionString("Bethlem"); /* = "Server=localhost;Username=postgres;Port=5432;Database=Bethlem;UserId=postgres;Password=postg1234;";*/

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                string selectUserByEmail = "select * from tegs where teg_title = @teg_title";
                using (NpgsqlCommand command = new NpgsqlCommand(selectUserByEmail, connection))
                {
                    command.Parameters.AddWithValue("@teg_title", tegTitle);
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            teg.Id = reader.GetInt32(0);//id
                            teg.tegTitle = reader.GetString(1);//teg_title                          
                        }
                        reader.Close();
                    }
                }
                connection.Close();
            }
            return teg;
        }
    }
}
