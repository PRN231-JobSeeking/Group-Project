using ClientRepository.Models;
using ClientRepository.Service;
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

        private readonly IPostService postService;

        private readonly IApplicationService applicationService;

        public PostDetailModel(IConfiguration config, IPostService postService, IApplicationService applicationService)
        {
            this.config = config;
            this.postService = postService;
            this.applicationService = applicationService;
        }
        public async void OnGet(int id)
        {
            Post = postService.GetModelAsync(id).Result;
        }

        public async void OnPostApply(int id, IFormFile cvFile)
        {
    
            if (cvFile == null) return;
            Trace.WriteLine(id + " " + cvFile.FileName);
            Post = postService.GetModelAsync(id).Result;
            Trace.WriteLine(applicationService.Create(id, cvFile).Result);
        }

        //public static void AddTokenHeader(this HttpClient client, string? token)
        //{
        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //}

        //client.AddTokenHeader(HttpContext.Session.GetString("token"));
    }
}
