using FY111.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FY111.Extensions
{
    public static class ServiceExtensions
    {
        // Identity設定
        //public static void ConfigureIdentity(this IServiceCollection services)
        //{
        //    var builder = services.AddIdentityCore<User>(options =>
        //    {
        //        options.Password.RequireDigit = true;
        //        options.Password.RequireLowercase = false;
        //        options.Password.RequireUppercase = false;
        //        options.Password.RequireNonAlphanumeric = false;
        //        options.Password.RequiredLength = 6;
        //        options.User.RequireUniqueEmail = false;
        //    });
        //    builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), builder.Services);
        //    builder.AddEntityFrameworkStores<FY111DbContext>()
        //        .AddDefaultTokenProviders();
        //}
        // JWT設定
        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings.GetSection("secretKey").Value;
            services.AddAuthentication(opt => {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.GetSection("validIssuer").Value,
                    ValidAudience = jwtSettings.GetSection("validAudience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });
        }
    }

    
}
