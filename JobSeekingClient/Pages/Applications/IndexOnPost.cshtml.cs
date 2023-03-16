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
    public class IndexOnPost : PageModel
    {
        private readonly IApplicationService _applicationService;

        public IndexOnPost(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [BindProperty]
        public IList<ApplicationModel> Application { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int postId)
        {
            //user for role checking
            var roleId = HttpContext.Session.GetInt32("Role");

            if (roleId == null)
            {
                return RedirectToPage("/Auth/Login");
            }

            //user id for interview/application/post tracking
            var userId = HttpContext.Session.GetInt32("UserId");

            var _applicationList = new List<ApplicationModel>();
            var token = HttpContext.Session.GetString("token");

            if(roleId == 2)
            {
                _applicationList = await _applicationService.GetListAsync(path: StoredURI.Application, expression: c => c.IsDeleted == false && c.PostId == postId, param: null, token: token);
            }
            if (_applicationList != null)
            {
                Application = _applicationList;
            }

            return Page();
        }
    }
}
