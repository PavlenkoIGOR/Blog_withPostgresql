using Blog.BLL;
using Blog.Data.Repositories;
using Blog_withPostgresql.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;


namespace Blog_withPostgresql
{
    public class Program
    {
        public static IServiceProvider ServiceProvider { get; private set; } //****���������
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // �������� ��������� ������ ����������� IUserRepo
            builder.Services.AddScoped<IUserRepo, IUserRepo>(); //****��������� ��� ����� ������������
            builder.Services.AddScoped<IPostRepo, PostRepo>(); //****��������� ��� ����� ������������
            builder.Services.AddScoped<IMyLogger, MyLogger>(); //****��������� ��� ����� ������������
            builder.Services.AddScoped<ITegRepo, TegRepo>(); //****��������� ��� ����� ������������
            builder.Services.AddScoped<IPostsTegsRepo, PostsTegsRepo>(); //****��������� ��� ����� ������������
            builder.Services.AddScoped<ICommentRepo, CommentRepo>(); //****��������� ��� ����� ������������



            // ��������� ������������ �����������
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