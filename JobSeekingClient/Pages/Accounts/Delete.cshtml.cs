using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ClientRepository.Service;
using ClientRepository.Service.Implementation;
using ClientRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using ClientRepository.Models;
using NuGet.Common;
using System.IO;
using ClientRepository.Utils;

namespace JobSeekingClient.Pages.Accounts
{
    public class DeleteModel : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly IInterviewService _interviewService;

        public DeleteModel(IAccountService accountService, IInterviewService interviewService)
        {
            _accountService = accountService;
            _interviewService = interviewService;
        }

        [BindProperty]
        public AccountModel Account { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            string? token = HttpContext.Session.GetString("token");
            int? test = HttpContext.Session.GetInt32("Role");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/Auth/Login");
            }
            if (test != 1)
            {
                return RedirectToPage("./HomePage");
            }
            string path = StoredURI.Account + "/" + id;
            var find = await _accountService.GetModelAsync(path: path, expression: c => c.IsDeleted == false, token: token);
            if (find == null)
            {
                return NotFound();
            }
            Account = find;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            string path = StoredURI.Account + "/" + id;
            string? token = HttpContext.Session.GetString("token");
          
            var find = await _accountService.GetModelAsync(path: path, expression: c => c.IsDeleted == false, token: token);
            find.IsDeleted = true;
            if(find.RoleId==(int)AccountRole.Applicant)
            {
                var findinterview = await _interviewService.GetListAsync(path: StoredURI.Interviews, expression: c => c.Application.ApplicantId == find.Id && c.Date > DateTime.Now, token: token);
                if (findinterview.Count > 0)
                {
                    Account = find;
                    ViewData["message"] = "Applicant has not done interviews yet!";
                    return Page();
                }
            }
            if (find.RoleId == (int)AccountRole.Interviewer)
            {
                var findinterview = await _interviewService.GetListAsync(path: StoredURI.Interviews, expression: c => c.Interviewer.Id == find.Id , token: token);
                if (findinterview.Find(c=> c.Date > DateTime.Now) != null)
                {
                    Account = find;
                    ViewData["message"] = "Interviewer has not done interview yet!";
                    return Page();
                }
                if (findinterview.Find(c=>c.Point==0 && string.IsNullOrEmpty(c.Feedback)) != null)
                {
                    Account = find;
                    ViewData["message"] = "Interviewer has not graded some interviews yet!";
                    return Page();
                }
            }

            if (id == null)
            {
                return NotFound();
            }
            if (id != Account.Id)
            {
                return BadRequest();
            }
            await _accountService.Update(find, path: StoredURI.Account + "/" + Account.Id.ToString(), token: token);
            return RedirectToPage("./IndexAdmin");
        }
    }
}
