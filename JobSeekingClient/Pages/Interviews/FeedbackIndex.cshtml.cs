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
using ClientRepository.Utils;
using ClientRepository;
using ClientRepository.Models;
using ClientRepository.Service.Implementation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace JobSeekingClient.Pages.Interviews
{
    public class FeedbackIndexModel : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly ISlotService _slotService;
        private readonly IInterviewService _interviewService;

        public FeedbackIndexModel(IAccountService accountService, ISlotService slotService, IInterviewService interviewService)
        {
            _accountService = accountService;
            _slotService = slotService;
            _interviewService = interviewService;
        }

        public IList<InterviewModel> Interviews { get;set; } = default!;
        public IEnumerable<SelectListItem> Slots { get; set; }


        public async Task<IActionResult> OnPostAsync(int slot)
        {
            if(slot <= 0)
            {
                return RedirectToPage("../Interviews/FeedbackIndex");
            }
            string? token = HttpContext.Session.GetString("token");
            int? role = HttpContext.Session.GetInt32("Role");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("../Auth/Login");
            }
            if (role != (int)AccountRole.Interviewer)
            {
                return RedirectToPage("../HomePage");
            }
            int? userId = HttpContext.Session.GetInt32("UserId");
            if(userId < 0)
            {
                return RedirectToPage("../Auth/Login");
            }
            var slots = await _slotService.GetListAsync(path: StoredURI.Slot, token: token);
            Slots = slots.Select(s => new SelectListItem()
            {
                Value = s.Id.ToString(),
                Text = $"Slot {s.Id}",
            });
            Interviews = await _interviewService.GetListAsync(path: StoredURI.Interviews + $"/Nonfeedbacks/InterviewerId/{userId}/SlotId/{slot}", token: token);
            return Page();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            string? token = HttpContext.Session.GetString("token");
            int? role = HttpContext.Session.GetInt32("Role");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("../Auth/Login");
            }
            if (role != (int)AccountRole.Interviewer)
            {
                return RedirectToPage("../HomePage");
            }
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId < 0)
            {
                return RedirectToPage("../Auth/Login");
            }
            var slots = await _slotService.GetListAsync(path: StoredURI.Slot, token: token);
            Slots = slots.Select(s => new SelectListItem()
            {
                Value = s.Id.ToString(),
                Text = $"Slot {s.Id}",
            });
            Interviews = await _interviewService.GetListAsync(path: StoredURI.Interviews + $"/Nonfeedbacks/InterviewerId/{userId}", token: token);
            return Page();
        }
    }
}
