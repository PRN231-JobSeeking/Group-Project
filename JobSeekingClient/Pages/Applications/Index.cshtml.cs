using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AppCore;
using AppCore.Models;
using ClientRepository.Service;
using ClientRepository;
using ClientRepository.Models;

namespace JobSeekingClient.Pages.Applications
{
    public class IndexModel : PageModel
    {
        private readonly IApplicationService _applicationService;

        public IndexModel(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        public IList<ApplicationModel> Application { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            //user for role checking
            var roleId = HttpContext.Session.GetInt32("Role");

            if (roleId != 1 || roleId != 2)
            {
                return RedirectToPage("/home");
            }

            var token = HttpContext.Session.GetString("token");
            var _applicationList = await _applicationService.GetListAsync(path: StoredURI.Application, expression: c => c.IsDeleted == false, param: null, token: token);

            if (_applicationList != null)
            {
                Application = _applicationList;
            }

            return Page();
        }
    }
}
