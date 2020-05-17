using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using RapidCMS.Core.Abstractions.Config;
using RapidCMS.Core.Abstractions.Dispatchers;
using RapidCMS.Core.Abstractions.Resolvers;
using RapidCMS.Core.Abstractions.Services;
using RapidCMS.Core.Authorization;
using RapidCMS.Core.Dispatchers;
using RapidCMS.Core.Models.Config;
using RapidCMS.Core.Resolvers.Repositories;
using RapidCMS.Core.Services.Parent;
using RapidCMS.Core.Services.Persistence;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class RapidCMSMiddleware
    {
        /// <summary>
        /// Use this method to setup the Repository APIs to support RapidCMS WebAssenbly on a separate server.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IServiceCollection AddRapidCMSApi(this IServiceCollection services, Action<IApiConfig>? config = null)
        {
            var rootConfig = GetRootConfig(config);

            services.AddSingleton(rootConfig);

            services.AddHttpContextAccessor();

            if (rootConfig.AllowAnonymousUsage)
            {
                services.AddSingleton<IAuthorizationHandler, AllowAllAuthorizationHandler>();
                services.AddSingleton<AuthenticationStateProvider, AnonymousAuthenticationStateProvider>();
            }

            services.AddTransient<IRepositoryResolver, ApiRepositoryResolver>();

            services.AddTransient<IParentService, ParentService>();

            return services;
        }

        private static ApiConfig GetRootConfig(Action<IApiConfig>? config = null)
        {
            var rootConfig = new ApiConfig();
            config?.Invoke(rootConfig);
            return rootConfig;
        }
    }
}
