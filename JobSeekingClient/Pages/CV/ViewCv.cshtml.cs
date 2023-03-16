using AppCore.Models;
using ClientRepository;
using ClientRepository.Models;
using ClientRepository.Service;
using ClientRepository.Utils;
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

        public async Task<IActionResult> OnGet(int id)
        {
            string? token = HttpContext.Session.GetString("token");
            int? role = HttpContext.Session.GetInt32("Role");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("../Auth/Login");
            }
            if (role == (int)AccountRole.Administrator)
            {
                return RedirectToPage("../Home");
            }
            var application = await applicationService.GetModelAsync(path: StoredURI.Application + $"/Get/Id/{id}", token: token);
            if(application == null)
            {
                return RedirectToPage("../Home");
            }
            Application = application;
            return Page();
        }
    }
}
