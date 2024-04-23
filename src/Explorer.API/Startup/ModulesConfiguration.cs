using Explorer.Blog.Infrastructure;
using Explorer.Payments.Infrastructure;
using Explorer.Stakeholders.Infrastructure;

namespace Explorer.API.Startup;

public static class ModulesConfiguration
{
    public static IServiceCollection RegisterModules(this IServiceCollection services)
    {
        services.ConfigureStakeholdersModule();
        services.ConfigureBlogModule();
        services.ConfigurePaymentsModule();
        services.AddHttpClient();

        return services;
    }
}