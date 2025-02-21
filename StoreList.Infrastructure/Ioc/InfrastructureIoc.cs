using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StoreList.Domain.Interfaces;
using StoreList.Infrastructure.Repositories;
using StoreList.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using StoreList.Domain.Entities;

namespace StoreList.Infrastructure.Ioc
{
    public static class InfrastructureIoc
    {

        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IShoppingListRepository, ShoppingListRepository>();

            services.AddIdentityCore<AppUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddSignInManager<SignInManager<AppUser>>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var secret = configuration["JwtConfig:Secret"];
                var issuer = configuration["JwtConfig:ValidIssuer"];
                var audience = configuration["JwtConfig:ValidAudiences"];

                if (secret is null || issuer is null || audience is null)
                {
                    throw new ApplicationException("Jwt is not set in the configuration");
                }
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidIssuer = issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
                };
            });
        }
    }
}
