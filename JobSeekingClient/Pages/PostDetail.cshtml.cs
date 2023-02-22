using JobSeekingClient.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Diagnostics;
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

        public void OnPostApply(int id, IFormFile cvFile)
        {
            if (cvFile == null) return;
            HttpClient client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, $"{config["ApiURI"]}Post/{id}");
            HttpResponseMessage responseMessage = client.SendAsync(requestMessage).Result;
            HttpContent postContent = responseMessage.Content;

            Trace.WriteLine(id + " " + cvFile.FileName);
            Post = (PostDTO)postContent.ReadFromJsonAsync(typeof(PostDTO)).Result;

            var applyReq = new ApplyRequest()
            {
                CvFile = cvFile,
                PostId = 1
            };
            var requestContent = new StringContent(JsonConvert.SerializeObject(applyReq), Encoding.UTF8, "multipart/form-data");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));
            var response = client.PostAsync($"{config["ApiURI"]}Application/Create", requestContent);
            Trace.WriteLine(response.Status.ToString());
            if(response.IsCompletedSuccessfully)
            {
                var content = response.Result.Content.ReadAsStringAsync().Result;
                var createdCompany = JsonConvert.DeserializeObject<bool>(content);
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
