using Microsoft.Extensions.DependencyInjection;
using VyshKat.Domain;
using VyshKat.Services;

namespace VyshKat;

public static class DependencyInjection
{
    public static IServiceCollection AddVyshKat(this IServiceCollection services) => services
        .AddSingleton<Fleet>()
        .AddSingleton<IServiceCenter>(new ServiceCenter())
        .AddSingleton<IBeginnerPolicy>(new SimplicityBeginnerPolicy())
        .AddSingleton<FleetService>()
        .AddSingleton<FleetReports>();
}
