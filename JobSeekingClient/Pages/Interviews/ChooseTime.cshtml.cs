using ClientRepository;
using ClientRepository.Models;
using ClientRepository.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;

namespace JobSeekingClient.Pages.Interviews
{
    public class ChooseTimeModel : PageModel
    {
        private readonly ISlotService _slotService;
        private readonly IInterviewService _interviewService;
        public ChooseTimeModel(ISlotService slotService, IInterviewService interviewService)
        {
            _slotService = slotService;
            _interviewService = interviewService;
        }

        [BindProperty]
        [Required]
        public string Date { get; set; }
        [BindProperty]
        [Required]
        public int SlotId { get; set; } 
        [BindProperty]
        [Required]
        public int ApplicationId { get; set; }
        public async Task<IActionResult> OnGetAsync(int applicationId)
        {
            var slots = await _slotService.GetListAsync(path: StoredURI.Slot, token: HttpContext.Session.GetString("token"));
            ViewData["SlotId"] = new SelectList(slots, nameof(SlotModel.Id), nameof(SlotModel.StartTime));
            Date = DateOnly.FromDateTime(DateTime.Today).AddDays(1).ToString("MM/dd/yyyy");
            if (applicationId <= 0)
            {
                return RedirectToPage("/Index");
            }
            ApplicationId = applicationId;
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (CheckValidValues())
            {
                string? token = HttpContext.Session.GetString("token");
                try
                {
                    var check = await _interviewService.GetAvailableInterviewers(SlotId, DateOnly.Parse(Date), ApplicationId, token);
                    if (check != null && check.Count() > 0)
                    {
                        return RedirectToPage("./Create", new { date = Date, slotId = SlotId, applicationId = ApplicationId });
                    }
                    else
                    {
                        ViewData["Message"] = "No interviewers is available at that moment!";
                    }
                } catch
                {
                    ViewData["Message"] = "Already had an meeting for this application at selected time or later!";
                }
            } else
            {
                ViewData["Message"] = "Interview Date must be 1 day after today!";
                
            }
            return await OnGetAsync(ApplicationId);
        }
        private bool CheckValidValues()
        {
            var result = false;
            if (SlotId > 0 && ApplicationId > 0 && Date != null && Date != "")
            {
                var date = DateOnly.Parse(Date);
                result = date.CompareTo(DateOnly.FromDateTime(DateTime.Today)) >= 1;                
            }
            return result;
        }
    }
}
