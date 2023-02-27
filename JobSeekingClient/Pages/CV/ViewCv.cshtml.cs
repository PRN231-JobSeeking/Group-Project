using AppCore.Models;
using ClientRepository.Models;
using ClientRepository.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JobSeekingClient.Pages
{
    public class ViewCvModel : PageModel
    {
        public readonly IConfiguration config;

        public readonly IApplicationService applicationService;
        public ViewCvModel(IConfiguration config, IApplicationService applicationService)
        {
            this.config = config;
            this.applicationService = applicationService;
        }

        public ApplicationModel Application { get; set; }

        public void OnGet(int id)
        {
            var application = applicationService.GetModelAsync(id).Result;
            if(application == null)
            {
                return;
            }
            Application = application;
        }
    }
}
