using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Hosting.Server.Features;
using System.Security.Cryptography;

namespace _4Layer.Api.Extensions;

public static class PortSetup
{
    public static IServiceCollection AddPortSetup(this IServiceCollection services)
    {
        return services;
    }

    public static IApplicationBuilder UsePortSetup(this IApplicationBuilder app)
    {
        var lifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

        lifetime.ApplicationStarted.Register(() =>
        {
            var addresses = app.ServerFeatures.Get<IServerAddressesFeature>()?.Addresses;

            Console.WriteLine("🔍 Checking API addresses...");

            if (addresses != null && addresses.Any())
            {
                foreach (var address in addresses)
                {
                    Console.WriteLine($"🚀 API is running at: {address}");
                    Console.WriteLine($"📚 Swagger UI available at: {address}/swagger\n");
                }

                var environment = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
            }
            else
            {
                Console.WriteLine("⚠️ No server addresses found.");
            }
        });

        return app;
    }

}