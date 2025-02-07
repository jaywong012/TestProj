using Microsoft.Extensions.DependencyInjection;
using NewProject.Services.Interface;
using NewProject.Services.Services;

namespace NewProject.Services;

public static class DependencyInjection
{
    public static void AddRegisterServicesDependency(this IServiceCollection services)
    {
        services.AddScoped<IXApiServices, XApiServices>();
        services.AddScoped<IFacebookApiService, FacebookApiService>();
    }
}