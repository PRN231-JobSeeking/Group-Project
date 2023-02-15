using ClientRepository.Extension;
using ClientRepository.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ClientRepository.Service.Implementation
{
    public class AuthenService : BaseService<UserLogin>, IAuthenService
    {
        public AuthenService(IHttpClientFactory clientFactory) : base(clientFactory)
        {

        }

        public virtual async Task<string> Login(UserLogin model)
        {
            var result = "";
            var response = await Client.PostAsJsonAsync(StoredURI.Login, model);
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsStringAsync().Result;
            }
            return result;
        }
    }
}
