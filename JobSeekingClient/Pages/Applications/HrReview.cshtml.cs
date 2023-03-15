using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClientRepository;
using NuGet.Common;
using ClientRepository.Service;
using ClientRepository.Service.Implementation;
using ClientRepository.Models;
using System.Diagnostics;
using AppCore.Models;
using Newtonsoft.Json.Linq;

namespace JobSeekingClient.Pages.Applications
{
    public class EditModel : PageModel
    {
        private readonly IApplicationService _applicationService;
        private readonly IAccountService _accountService;
        private readonly IInterviewService _interviewService;
        private readonly IPostService _postService;

        public EditModel(IApplicationService applicationService, IAccountService accountService, IInterviewService interviewService,
            IPostService postService)
        {
            _applicationService = applicationService;
            _accountService = accountService;
            _interviewService = interviewService;
            _postService = postService;
        }

        [BindProperty]
        public ApplicationModel Application { get; set; } = default!;

        [BindProperty]
        public IList<InterviewModel> Interview { get; set; } = default!;

        [BindProperty]
        public string ApplicationStatus { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            string? token = HttpContext.Session.GetString("token");

            var roleId = HttpContext.Session.GetInt32("Role");

            if (roleId == null || roleId == 4)
            {
                return RedirectToPage("/home");
            }

            var userId = HttpContext.Session.GetInt32("UserId");

            if (id == null)
            {
                return NotFound();
            }


            //getting application
            string path = StoredURI.Application + "/Get/Id/" + id;
            var find = await _applicationService.GetModelAsync(path: path, expression: c => c.IsDeleted == false, token: token);
            if (find == null)
            {
                Debug.WriteLine("Application.Edit.OnGetAsync: Failed to get application");
                return NotFound();
            }
            Application = find;

            //getting relevant interview list
            string path2 = StoredURI.Interviews;
            var find2 = await _interviewService.GetListAsync(path: path2, expression: c => c.IsDeleted == false && c.ApplicationId == Application.Id, token: token);
            if (find2 == null)
            {
                Debug.WriteLine("Application.Edit.OnGetAsync: Failed to get interview list");
                return NotFound();
            }
            Debug.WriteLine("Application.Edit.OnGetAsync: Got " + find2.Count + " relevant interviews.");
            Interview = find2;


            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            switch (ApplicationStatus)
            {
                case "1":
                    {
                        Application.Status = true;
                        break;
                    }
                case "0":
                    {
                        Application.Status = false;
                        break;
                    }
                case "null":
                    {
                        Application.Status = null;
                        break;
                    }
                default:
                    {
                        return Page();
                    }
            }

            string? token = HttpContext.Session.GetString("token");
            await _applicationService.Update(Application, path: StoredURI.Application + "/" + Application.Id.ToString(), token: token);

            PostDTO post = _postService.GetModelAsync(path: StoredURI.Post + "" + Application.PostId, token: token).Result;

            if (post.Amount == 0)
            {
                return RedirectToPage("./Index");
            }

            post.Amount--;

            await _postService.Update(post, path: StoredURI.Post, token: token);

            return RedirectToPage("./Index");
        }

    }
}
