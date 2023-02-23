using AppCore.Models;
using JobSeekingClient.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JobSeekingClient.Pages
{
    public class ViewCvModel : PageModel
    {
        public readonly IConfiguration config;
        public ViewCvModel(IConfiguration config)
        {
            this.config = config;
        }

        public string CvPath { get; set; }

        public void OnGet(int id)
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, $"{config["ApiURI"]}Application/Get/Id/{id}");
            HttpResponseMessage responseMessage = client.SendAsync(requestMessage).Result;
            HttpContent content = responseMessage.Content;
            var application = (Application)content.ReadFromJsonAsync(typeof(Application)).Result;
            if(application != null)
            {
                CvPath = application.CV;
            }
        }
    }
}
