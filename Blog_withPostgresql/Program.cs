using Blog.BLL;
using Blog.Data.Repositories;
using Blog_withPostgresql.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;


namespace Blog_withPostgresql
{
    public class Program
    {
        public static IServiceProvider ServiceProvider { get; private set; } //****добавлено
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Получить экземпляр вашего репозитория IUserRepo
            builder.Services.AddScoped<IUserRepo, IUserRepo>(); //****добавлено для своих репозиториев
            builder.Services.AddScoped<IPostRepo, PostRepo>(); //****добавлено для своих репозиториев
            builder.Services.AddScoped<IMyLogger, MyLogger>(); //****добавлено для своих репозиториев
            builder.Services.AddScoped<ITegRepo, TegRepo>(); //****добавлено для своих репозиториев
            builder.Services.AddScoped<IPostsTegsRepo, PostsTegsRepo>(); //****добавлено для своих репозиториев
            builder.Services.AddScoped<ICommentRepo, CommentRepo>(); //****добавлено для своих репозиториев



            // установка конфигурации подключения
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie("Cookies");

            builder.Services.AddAuthorization();

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

            
            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseMiddleware<Midelleware>();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                await next.Invoke();
            });

            app.UseMiddleware< MyLoggingMiddleware >();

            app.Run();
        }
    }
}