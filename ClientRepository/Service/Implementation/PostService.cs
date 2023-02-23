using ClientRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ClientRepository.Service.Implementation
{
    public class PostService : BaseService<PostDTO>, IPostService
    {
        public PostService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
        }

        public async Task<PostDTO> GetModelAsync(int id)
        {
            var response = await Client.GetAsync(StoredURI.Post + $"/{id}");
            if(response.IsSuccessStatusCode)
            {
                HttpContent content = response.Content;
                return (PostDTO)content.ReadFromJsonAsync(typeof(PostDTO)).Result;
            }
            return null;
        }
    }
}
