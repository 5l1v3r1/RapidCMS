using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using RapidCMS.Core.Abstractions.Data;
using RapidCMS.Core.Abstractions.Forms;
using RapidCMS.Core.Repositories;
using RapidCMS.Repositories.ApiBridge.Models;

namespace RapidCMS.Repositories.ApiBridge
{
    public class ApiRepository<TEntity> : BaseRepository<TEntity>
        where TEntity : class, IEntity
    {
        private readonly HttpClient _httpClient;

        // TODO: parents and IQueries + handle 400/401/404 + handle empty responses

        public ApiRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
            Console.WriteLine(_httpClient.BaseAddress);
        }

        private HttpRequestMessage CreateRequest(HttpMethod method, string url)
        {
            return new HttpRequestMessage(method, url);
        }

        private HttpRequestMessage CreateRequest<T>(HttpMethod method, string url, T content)
        {
            if (method == HttpMethod.Get)
            {
                throw new InvalidOperationException();
            }

            var request = CreateRequest(method, url);
            request.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

            return request;
        }

        public override async Task DeleteAsync(string id, IParent? parent)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"entity/{id}");
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public override async Task<IEnumerable<TEntity>> GetAllAsync(IParent? parent, IQuery<TEntity> query)
        {
            var request = CreateRequest(HttpMethod.Post, "all", new QueryModel(parent, query));
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<TEntity>>(json);
        }

        public override async Task<TEntity?> GetByIdAsync(string id, IParent? parent)
        {
            var request = CreateRequest(HttpMethod.Post, $"entity/{id}", new QueryModel(parent));
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TEntity>(json);
        }

        public override async Task<TEntity?> InsertAsync(IEditContext<TEntity> editContext)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "entity")
            {
                Content = new StringContent(JsonConvert.SerializeObject(editContext.Entity), Encoding.UTF8, "application/json")
            };
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TEntity>(json);
        }

        public override async Task<TEntity> NewAsync(IParent? parent, Type? variantType = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"new");
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TEntity>(json);
        }

        public override async Task UpdateAsync(IEditContext<TEntity> editContext)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"entity/{editContext.Entity.Id}")
            {
                Content = new StringContent(JsonConvert.SerializeObject(editContext.Entity), Encoding.UTF8, "application/json")
            };
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }
    }
}
