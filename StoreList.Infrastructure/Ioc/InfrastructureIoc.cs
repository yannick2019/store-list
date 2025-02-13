using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StoreList.Domain.Interfaces;
using StoryList.Infrastructure.Repositories;

namespace StoreList.Infrastructure.Ioc
{
    public static class InfrastructureIoc
    {

        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IShoppingListRepository, ShoppingListRepository>();
        }
    }
}
