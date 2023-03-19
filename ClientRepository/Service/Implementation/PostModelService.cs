using ClientRepository.Extension;
using ClientRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientRepository.Service.Implementation
{
    public class PostModelService : BaseService<PostModel>, IPostModelService
    {
        public PostModelService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
        }
        public override async Task<bool> Delete(PostModel model, string? path = null, Dictionary<string, string>? param = null, string? token = null)
        {
            var result = false;
            AddTokenHeader(token);
            var response = await Client.DeleteAsync(path + HttpRequestSupport.GetQueryPath(param));
            if (response.IsSuccessStatusCode)
            {
                result = true;
            } else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
            return result;
        }
    }
}
