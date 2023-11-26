using Blog.BLL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Blog_withPostgresql;

public class Midelleware
{
    private readonly RequestDelegate _next;

    public Midelleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last().Trim();

        if (token == null)
        {
            //await AttachContext(context, token);
        }
    }
    public string GenerateJWT()
    {
        var jwt = new JwtSecurityTokenHandler();
        var descriptor = new SecurityTokenDescriptor()
        {
        };
        SecurityToken securityToken = jwt.CreateToken(descriptor);

        return jwt.WriteToken(securityToken);
    }


    /*
     * from Afanasiev_D_V
    private async Task AttachContext(HttpContext context, string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("Ключ");

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = "Тут издатель",
                ValidateAudience = true,
                ValidAudience = "Для кого был выдан",
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = true
            };

            tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            int accountId = int.Parse(jwtToken.Claims.First(x => x.Type == "UserId").Value);

            //var client = await GetUserByEmail();
            //if (client == null)
            //{
            //    return;
            //}

            //context.Items["LoggedInUser"] = client;
        }
        catch
        {
            //тут            
        }
    }
    */
}

/*
 public class TokenService
{
private const string Secret = "your_secret_key_here";

public string GenerateToken(string username)
{
var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

var token = new JwtSecurityToken(
issuer: "your_issuer_here",
audience: "your_audience_here",
claims: new[] { new System.Security.Claims.Claim("username", username) },
expires: DateTime.Now.AddHours(1),
signingCredentials: credentials);

return new JwtSecurityTokenHandler().WriteToken(token);
}
}
 */