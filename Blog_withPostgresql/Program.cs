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
        public static IServiceProvider ServiceProvider { get; private set; } //****���������
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // �������� ��������� ������ ����������� IUserRepo
            builder.Services.AddScoped<IUserRepo, UserRepo>(); //****��������� ��� ����� ������������

            //******����������� JWT-�������
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
            //            // ���������, ����� �� �������������� �������� ��� ��������� ������
            //            ValidateIssuer = true,
            //            // ������, �������������� ��������
            //            ValidIssuer = AuthOptions.ISSUER,
            //            // ����� �� �������������� ����������� ������
            //            ValidateAudience = true,
            //            // ��������� ����������� ������
            //            ValidAudience = AuthOptions.AUDIENCE,
            //            // ����� �� �������������� ����� �������������
            //            ValidateLifetime = true,
            //            // ��������� ����� ������������
            //            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            //            // ��������� ����� ������������
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