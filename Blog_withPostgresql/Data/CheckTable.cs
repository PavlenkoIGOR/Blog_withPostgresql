using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Blog_withPostgresql.Data
{
    public class ChaeckTable
    {
        static IConfiguration _configuration;
        static string connectionString = _configuration.GetConnectionString("Bethlam");
        public ChaeckTable(IConfiguration configuration) 
        {
            _configuration = configuration;
        }    
        public static bool IsTableExists(string tableName)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new NpgsqlCommand())
                {
                    command.Connection = connection;

                    // Подготовка SQL-запроса
                    command.CommandText = "SELECT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = @tableName)";

                    // Параметр tableName
                    command.Parameters.AddWithValue("@tableName", tableName);

                    // Выполнение SQL-запроса
                    bool isExists = (bool)command.ExecuteScalar();

                    return isExists;
                }
            }
        }
    }
}
