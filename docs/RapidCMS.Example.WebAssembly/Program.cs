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
using RapidCMS.Core.Repositories;
using RapidCMS.Example.Shared.DataViews;
using RapidCMS.Example.Shared.Handlers;
using RapidCMS.Core.Abstractions.Setup;
using Blazored.LocalStorage;
using RapidCMS.Repositories.ApiBridge;

namespace RapidCMS.Example.WebAssembly
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddScoped(sp => 
                new HttpClient { BaseAddress = new Uri("https://localhost:44396/api/person/") });

            builder.Services.AddAuthorizationCore();

            builder.Services.AddBlazoredLocalStorage();

            builder.Services.AddScoped<BaseRepository<Person>, ApiRepository<Person>>();
            builder.Services.AddSingleton<BaseRepository<ConventionalPerson>, InMemoryRepository<ConventionalPerson>>();
            builder.Services.AddSingleton<BaseRepository<Country>, InMemoryRepository<Country>>();
            builder.Services.AddSingleton<BaseRepository<User>, InMemoryRepository<User>>();
            builder.Services.AddSingleton<BaseRepository<TagGroup>, InMemoryRepository<TagGroup>>();
            builder.Services.AddSingleton<BaseRepository<Tag>, InMemoryRepository<Tag>>();

            builder.Services.AddSingleton<MappedBaseRepository<MappedEntity, DatabaseEntity>, MappedInMemoryRepository<MappedEntity, DatabaseEntity>>();
            builder.Services.AddSingleton<IConverter<MappedEntity, DatabaseEntity>, Mapper>();
            builder.Services.AddSingleton<DatabaseEntityDataViewBuilder>();

            builder.Services.AddSingleton<RandomNameActionHandler>();
            builder.Services.AddSingleton<Base64TextFileUploadHandler>();
            builder.Services.AddSingleton<Base64ImageUploadHandler>();

            builder.Services.AddRapidCMS(config =>
            {
                config.AllowAnonymousUser();

                config.SetCustomLoginStatus(typeof(LoginStatus));

                config.AddPersonCollection();

                config.AddCountryCollection();

                config.AddPage("beaker", "Some random page", config =>
                {
                    config.AddSection(typeof(CustomSection));
                    config.AddSection("country", edit: false);
                });

                config.AddUserCollection();

                config.AddTagCollection();

                config.AddMappedCollection();

                config.AddConventionCollection();

                config.Dashboard.AddSection(typeof(DashboardSection));
                config.Dashboard.AddSection("user", edit: true);
            });

            var host = builder.Build();

            // TODO
            var cmsOptions = host.Services.GetService<ICms>();
            cmsOptions.IsDevelopment = true;

            await host.RunAsync();
        }
    }
}
