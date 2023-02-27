using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ClientRepository.Service;
using ClientRepository.Models;
using AppCore.Models;
using ClientRepository;

namespace JobSeekingClient.Pages.Interviews
{
    public class CreateModel : PageModel
    {
        private readonly IInterviewService _interviewService;

        public CreateModel(IInterviewService interviewService)
        {
            _interviewService= interviewService;
        }
        public async Task<IActionResult> OnGetAsync(string date, int slotId, int applicationId)
        {
            Interview = new InterviewModel()
            {
                ApplicationId = applicationId,
                SlotId = slotId,
                Date = DateTime.Parse(date),
            };
            string? token = HttpContext.Session.GetString("token");
            var interviewers = await _interviewService.GetAvailableInterviewers(slotId, DateOnly.Parse(date), Interview.ApplicationId, token);
            ViewData["InterviewerId"] = new SelectList(interviewers, "Id", nameof(AccountModel.Email));
            return Page();
        }

        [BindProperty]
        public InterviewModel Interview { get; set; } = null!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid)
            {
                return await OnGetAsync(Interview.Date.ToString(), Interview.SlotId, Interview.ApplicationId);
            }
            string? token = HttpContext.Session.GetString("token");
            await _interviewService.Add(Interview, path: StoredURI.Interviews, token: token);

            return RedirectToPage("./Index");
        }
    }
}
