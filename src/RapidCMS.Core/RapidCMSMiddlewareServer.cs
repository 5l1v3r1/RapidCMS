using System;
using System.Threading;
using RapidCMS.Core.Abstractions.Config;
using RapidCMS.Core.Abstractions.Services;
using RapidCMS.Core.Services.Auth;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class RapidCMSMiddleware
    {
        /// <summary>
        /// Use this method to configure RapidCMS to run on a Blazor Server App, fully server side.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IServiceCollection AddRapidCMSServer(this IServiceCollection services, Action<ICmsConfig>? config = null)
        {
            var rootConfig = GetRootConfig(config);

            services.AddTransient<IAuthService, ServerSideAuthService>();

            // Semaphore for repositories
            services.AddSingleton(serviceProvider => new SemaphoreSlim(rootConfig.Advanced.SemaphoreCount, rootConfig.Advanced.SemaphoreCount));

            return services.AddRapidCMSCore(rootConfig);
        }
    }
}
