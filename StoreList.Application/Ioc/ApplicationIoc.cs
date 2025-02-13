using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StoreList.Application.Interfaces;
using StoryList.Application.Services;

namespace StoreList.Application.Ioc
{
    public static class ApplicationIoc
    {
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IShoppingListService, ShoppingListService>();
        }

    }
}
