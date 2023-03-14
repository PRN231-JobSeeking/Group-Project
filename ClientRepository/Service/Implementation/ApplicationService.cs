﻿using ClientRepository.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ClientRepository.Service.Implementation
{
    public class ApplicationService : BaseService<ApplicationModel>, IApplicationService
    {
        public ApplicationService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
        }

        public async Task<bool> Create(int postId, IFormFile file, string? token)
        {
            MultipartFormDataContent content = new MultipartFormDataContent();
            StreamContent fileContent = new StreamContent(file.OpenReadStream());
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "file",
                FileName = file.FileName
            };
            content.Add(fileContent);
            StringContent nameContent = new StringContent($"{postId}");
            nameContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "postId"
            };
            AddTokenHeader(token);
            content.Add(nameContent);
            var response = await Client.PostAsync(StoredURI.Application + "/Create", content);
            if (response.IsSuccessStatusCode)
            {
                HttpContent nContent = response.Content;
                return (bool)nContent.ReadFromJsonAsync(typeof(bool)).Result;
            }
            return false;
        }
    }
}
