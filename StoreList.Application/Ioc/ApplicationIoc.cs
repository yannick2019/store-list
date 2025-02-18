using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StoreList.Application.Interfaces;
using StoreList.Application.Services;
using StoryList.Application.Services;

namespace StoreList.Application.Ioc
{
    public static class ApplicationIoc
    {
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IShoppingListService, ShoppingListService>();
            services.AddScoped<IAuthService, AuthService>();

        }
    }
}
