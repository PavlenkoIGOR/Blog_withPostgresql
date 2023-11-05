using Blog_withPostgresql.Controllers;
using Blog_withPostgresql.Repositories;

namespace Blog_withPostgresql
{
    public class Program
    {
        public static IServiceProvider ServiceProvider { get; private set; } //добавлено
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Получить экземпляр вашего репозитория IUserRepo
            builder.Services.AddScoped<IUserRepo, UserRepo>(); //добавлено для своих репозиториев

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

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}