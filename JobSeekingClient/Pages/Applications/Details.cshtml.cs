using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ClientRepository;
using ClientRepository.Models;
using ClientRepository.Service;
using ClientRepository.Service.Implementation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JobSeekingClient.Pages.Applications
{
    public class DetailsModel : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly IApplicationService _applicationService;
        private readonly IInterviewService _interviewService;

        public DetailsModel(IApplicationService applicationService, IAccountService accountService, IInterviewService interviewService)
        {
            _applicationService = applicationService;
            _accountService = accountService;
            _interviewService = interviewService;
        }

        public ApplicationModel Application { get; set; }
        public AccountModel Applicant { get; set; }

        public IList<InterviewModel> Interview { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            //user for role checking
            var roleId = HttpContext.Session.GetInt32("Role");

            if (roleId == null)
            {
                return RedirectToPage("/Auth/Login");
            }

            //token for API
            var token = HttpContext.Session.GetString("token");

            string path = StoredURI.Application + "/Get/Id/" + id;

            //getting application
            var find = await _applicationService.GetModelAsync(token: token, path: path);
            Debug.WriteLine("Details.OnGet: Getting appilcation");
            if (find == null)
            {
                Debug.WriteLine("Details.OnGet: Application not found");
                return NotFound();
            }
            Application = find;

            //getting account
            string path2 = StoredURI.Account + "/" + find.ApplicantId;
            Debug.WriteLine("Details.OnGet: path to account: " + path2);
            var find2 = await _accountService.GetModelAsync(token: token, path: path2);
            if (find2 == null)
            {
                Debug.WriteLine("Details.OnGet: Account not found");
                return NotFound();
            }
            Applicant = find2;

            //getting interviews
            string path3 = StoredURI.Interviews + "/application/" + find.Id;
            var find3 = await _interviewService.GetListAsync(token: token, path: path3);
            if (find3 == null)
            {
                Debug.WriteLine("Details.OnGet: Interview not found");
                return NotFound();
            }
            Debug.WriteLine("Details.OnGet: Got " + find3.Count() + " related interview.");
            Interview = find3;

            //if there's less than 2 interview, warning message to add more interview

            int count = 0;
            int count2 = 0;
            foreach (var item in find3)
            {
                count++;
                if (item.Feedback != null)
                {
                    count2++;
                }
            }

            if (count < 2)
            {
                if (find == null)
                {
                    ViewData["message1"] = "The application needs " + (2 - count) + " more interviews";
                }
            }

            //if there's enough rated interviews, and application has not yet been reviewed
            //warning message to approve/deny application
            if (count2 > 1 && find.Status == null)
            {
                ViewData["message2"] = "The application needs to be rated";
            }

            return Page();
        }
    }
}
