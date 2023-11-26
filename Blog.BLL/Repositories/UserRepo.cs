﻿using Blog.BLL.Models;
using Blog.BLL.ViewModels;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Security;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Blog_withPostgresql.Repositories
{
    public interface IUserRepo
    {
        public Task AddUser(UserRegViewModel userView);
        public User GetUserByEmail(string email);
        public User GetUserByPassword(string password);
        public User GetUserById(int id);
        public Task EditUser(UsersViewModel usersViewModel);
    }







    public class UserRepo : IUserRepo
    {
        public static IConfiguration _configuration;
        string connectionString = _configuration.GetConnectionString("Bethlem"); /* = "Server=localhost;Username=postgres;Port=5432;Database=Bethlem;UserId=postgres;Password=postg1234;";*/
        public UserRepo(IConfiguration configuration)
        {
            _configuration = configuration;
        }





        public async Task AddUser(UserRegViewModel userView)
        {
            string connectionString = _configuration.GetConnectionString("Bethlem"); /* = "Server=localhost;Username=postgres;Port=5432;Database=Bethlem;UserId=postgres;Password=postg1234;";*/
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                //using (var command = new NpgsqlCommand(sqlExpression, connection)) /************************************ так НЕ работает **************************/
                //{
                //    command.ExecuteNonQuery();
                //}
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Users (name, age, role, email, password) VALUES (@name, @age, @role, @email, @password)";
                    command.Parameters.AddWithValue("@name", userView.Name);
                    command.Parameters.AddWithValue("@email", userView.Email);
                    command.Parameters.AddWithValue("@password", PasswordHash.HashPassword(userView.Password));
                    command.Parameters.AddWithValue("@age", userView.Age);
                    command.Parameters.AddWithValue("@role", "user");
                    command.ExecuteNonQuery();
                }
                connection.CloseAsync().Wait();
            }
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
        public User GetUserById(int id)
        {
            string connectionString = _configuration.GetConnectionString("Bethlem"); /* = "Server=localhost;Username=postgres;Port=5432;Database=Bethlem;UserId=postgres;Password=postg1234;";*/
            User user = new User();
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                string selectUserByEmail = "select * from users where id = @id";
                using (NpgsqlCommand command = new NpgsqlCommand(selectUserByEmail, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
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

        public async Task EditUser(UsersViewModel usersVM)
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
