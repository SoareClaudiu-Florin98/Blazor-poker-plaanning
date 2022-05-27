using BlazorPokerPlanning.Shared.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorPokerPlanning.Shared
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            services.AddDbContext<PokerPlanningDbContext>(options => options.UseInMemoryDatabase(databaseName: "PokerPlanningDb"));

            return services;
        }
         
    }
}
