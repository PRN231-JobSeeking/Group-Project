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

        [BindProperty]
        public IList<ApplicationModel> Application { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
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

            switch (roleId)
            {
                case 1:
                    {
                        _applicationList = await _applicationService.GetListAsync(path: StoredURI.Application, expression: c => c.IsDeleted == false, param: null, token: token);
                        break;
                    }
                case 2:
                    {
                        _applicationList = await _applicationService.GetListAsync(path: StoredURI.Application, expression: c => c.IsDeleted == false, param: null, token: token);
                        break;
                    }
                case 3:
                    {
                        _applicationList = await _applicationService.GetListAsync(path: StoredURI.Application, expression: c => c.IsDeleted == false, param: null, token: token);
                        break;
                    }
                case 4:
                    {
                        _applicationList = await _applicationService.GetListAsync(path: StoredURI.Application, expression: c => c.IsDeleted == false && c.ApplicantId == userId, param: null, token: token);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            if (_applicationList != null)
            {
                Application = _applicationList;
            }

            return Page();
        }
    }
}
