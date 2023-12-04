using Blog.BLL;
using Blog.Data.Repositories;
using Blog_withPostgresql.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;


namespace Blog_withPostgresql
{
    public class Program
    {
        public static IServiceProvider ServiceProvider { get; private set; } //****���������
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // �������� ��������� ������ ����������� IUserRepo
            builder.Services.AddScoped<IUserRepo, UserRepo>(); //****��������� ��� ����� ������������
            builder.Services.AddScoped<IPostRepo, PostRepo>(); //****��������� ��� ����� ������������
            builder.Services.AddScoped<IMyLogger, MyLogger>(); //****��������� ��� ����� ������������
            builder.Services.AddScoped<ITegRepo, TegRepo>(); //****��������� ��� ����� ������������
            builder.Services.AddScoped<IPostsTegsRepo, PostsTegsRepo>(); //****��������� ��� ����� ������������

            //
            builder.Services.AddAuthorization();
            //builder.Services.AddAuthentication().AddCookie("BlogApplication_Cookie");

            /* ******����������� JWT-�������
            // ��������� JWT-��������������
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "your_issuer", // ���������� ���� �������� �������� (Issuer)
                        ValidAudience = "your_audience", // ���������� ���� �������� ��������� (Audience)
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_secret_key")) // ���������� ��� ���� �������
                    };
                });
            */

            // ��������� ������������ �����������
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => //CookieAuthenticationOptions
                {
                    options.LoginPath = new PathString("/AuthReg/AuthUser");
                });


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

            app.Run();
        }
    }
}