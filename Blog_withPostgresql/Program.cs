using Blog.BLL;
using Blog.Data.Repositories;
using Blog_withPostgresql.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;


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
            builder.Services.AddScoped<IPostRepo, PostRepo>(); //****добавлено для своих репозиториев
            builder.Services.AddScoped<IMyLogger, MyLogger>(); //****добавлено для своих репозиториев
            builder.Services.AddScoped<ITegRepo, TegRepo>(); //****добавлено для своих репозиториев
            builder.Services.AddScoped<IPostsTegsRepo, PostsTegsRepo>(); //****добавлено для своих репозиториев

            //
            builder.Services.AddAuthorization();
            //builder.Services.AddAuthentication().AddCookie("BlogApplication_Cookie");

            /* ******подключение JWT-токенов
            // Настройка JWT-аутентификации
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "your_issuer", // Установите ваше значение издателя (Issuer)
                        ValidAudience = "your_audience", // Установите ваше значение аудитории (Audience)
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_secret_key")) // Установите ваш ключ подписи
                    };
                });
            */

            // установка конфигурации подключения
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