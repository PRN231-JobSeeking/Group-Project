using JobSeekingClient.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace JobSeekingClient.Pages
{
    public class PostDetailModel : PageModel
    {
        public PostDTO Post { get; set; }

        private readonly IConfiguration config;

        public PostDetailModel(IConfiguration config)
        {
            this.config = config;
        }
        public void OnGet(int id)
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, $"{config["ApiURI"]}Post/{id}");
            HttpResponseMessage responseMessage = client.SendAsync(requestMessage).Result;
            HttpContent content = responseMessage.Content;
            Post = (PostDTO)content.ReadFromJsonAsync(typeof(PostDTO)).Result;
        }

        public async void OnPostApply(int id, IFormFile cvFile)
        {
            if (cvFile == null) return;
            HttpClient client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, $"{config["ApiURI"]}Post/{id}");
            HttpResponseMessage responseMessage = client.SendAsync(requestMessage).Result;
            HttpContent postContent = responseMessage.Content;

            Trace.WriteLine(id + " " + cvFile.FileName);
            Post = (PostDTO)postContent.ReadFromJsonAsync(typeof(PostDTO)).Result;
            MultipartFormDataContent content = new MultipartFormDataContent();
            StreamContent fileContent = new StreamContent(cvFile.OpenReadStream());
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "file",
                FileName = cvFile.FileName
            };
            content.Add(fileContent);
            StringContent nameContent = new StringContent($"{id}");
            nameContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "postId"
            };
            content.Add(nameContent);
            //var requestContent = new StringContent(JsonConvert.SerializeObject(applyReq), Encoding.UTF8, "application/json");
            //content.Add(requestContent, "data");
            var response = client.PostAsync($"{config["ApiURI"]}Application/Create", content);
            if(response.IsCompletedSuccessfully)
            {
                var contentRes = await (await response).Content.ReadAsStringAsync();
                var createdCompany = JsonConvert.DeserializeObject<bool>(contentRes);
                Trace.WriteLine(createdCompany);
            }
        }

        //public static void AddTokenHeader(this HttpClient client, string? token)
        //{
        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //}

        //client.AddTokenHeader(HttpContext.Session.GetString("token"));
    }
}
