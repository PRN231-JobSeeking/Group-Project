using ClientRepository.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ClientRepository.Service.Implementation
{
    public class BaseService<TModel> : IBaseService<TModel> where TModel : class
    {
        public HttpClient Client { get; set; } = null!;
        public BaseService(IHttpClientFactory clientFactory)
        {
            Client = clientFactory.CreateClient("BaseClient");
        }
        public virtual async Task<bool> Add(TModel model, string? path = null, string? token = null)
        {
            var result = false;
            AddTokenHeader(token);
            var response = await Client.PostAsJsonAsync(path, model);
            if (response.IsSuccessStatusCode)
            {
                result = true;
            }
            return result;
        }

        public virtual async Task<bool> Delete(TModel model, string? path = null, Dictionary<string, string>? param = null, string? token = null)
        {
            var result = false;
            AddTokenHeader(token);
            var response = await Client.DeleteAsync(path + HttpRequestSupport.GetQueryPath(param));
            if (response.IsSuccessStatusCode)
            {
                result = true;
            }
            return result;
        }

        public virtual async Task<List<TModel>?> GetListAsync(Func<TModel, bool>? expression = null, string? path = null, Dictionary<string, string>? param = null, string? token = null)
        {
            var result = new List<TModel>();
            AddTokenHeader(token);
            var response = await Client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadFromJsonAsync<List<TModel>>();
                if (expression != null && result != null)
                {
                    result = result.Where(expression).ToList();
                }
            }
            return result;
        }

        public virtual async Task<bool> Update(TModel model, string? path = null, Dictionary<string,string>? param = null, string? token = null)
        {
            var result = false;
            AddTokenHeader(token);
            var response = await Client.PutAsJsonAsync(path + HttpRequestSupport.GetQueryPath(param), model);
            if (response.IsSuccessStatusCode)
            {
                result = true;
            }
            return result;
        }
        public void AddTokenHeader(string? token = null)
        {
            if(token != null)
            {
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public virtual async Task<TModel?> GetModelAsync(Func<TModel, bool>? expression = null, string? path = null, Dictionary<string, string>? param = null, string? token = null)
        {
            TModel? result = null;
            AddTokenHeader(token);
            var response = await Client.GetAsync(path + HttpRequestSupport.GetQueryPath(param));
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadFromJsonAsync<TModel>();
            }
            return result;
        }
    }
}
