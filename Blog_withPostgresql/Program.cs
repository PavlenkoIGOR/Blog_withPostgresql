using Blog_withPostgresql.Controllers;
using Blog_withPostgresql.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Blog_withPostgresql
{
    public class Program
    {
        public static IServiceProvider ServiceProvider { get; private set; } //****добавлено
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Получить экземпляр вашего репозитория IUserRepo
            builder.Services.AddScoped<IUserRepo, UserRepo>(); //****добавлено для своих репозиториев

            //******подключение JWT-токенов
            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication("OAuth")
                .AddJwtBearer("OAuth", config => 
                {

                });


            //builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(options =>
            //    {
            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            // указывает, будет ли валидироваться издатель при валидации токена
            //            ValidateIssuer = true,
            //            // строка, представляющая издателя
            //            ValidIssuer = AuthOptions.ISSUER,
            //            // будет ли валидироваться потребитель токена
            //            ValidateAudience = true,
            //            // установка потребителя токена
            //            ValidAudience = AuthOptions.AUDIENCE,
            //            // будет ли валидироваться время существования
            //            ValidateLifetime = true,
            //            // установка ключа безопасности
            //            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            //            // валидация ключа безопасности
            //            ValidateIssuerSigningKey = true,
            //        };
            //    });


            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}