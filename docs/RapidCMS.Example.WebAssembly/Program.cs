using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RapidCMS.Example.Shared.Components;
using RapidCMS.Repositories;
using RapidCMS.Example.Shared.Data;
using RapidCMS.Example.Shared.Collections;

namespace RapidCMS.Example.WebAssembly
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            // builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddAuthorizationCore();

            builder.Services.AddSingleton<InMemoryRepository<Person>>();
            builder.Services.AddSingleton<InMemoryRepository<User>>();

            builder.Services.AddRapidCMS(config =>
            {
                config.AllowAnonymousUser();

                config.SetCustomLoginStatus(typeof(LoginStatus));

                config.AddPersonCollection();
                config.AddUserCollection();
            });

            await builder.Build().RunAsync();
        }
    }
}
