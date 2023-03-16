using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ClientRepository.Service;
using ClientRepository.Utils;
using ClientRepository;

namespace JobSeekingClient.Pages.Interviews
{
    public class FeedbackModel : PageModel
    {
        private readonly IInterviewService _interviewService;
        private readonly IApplicationService _applicationService;
        private readonly IFeedbackService _feedbackService;
        private readonly ISlotService _slotService;
        private readonly IAccountService _accountService;

        public FeedbackModel(IInterviewService interviewService, IFeedbackService feedbackService, ISlotService slotService, IApplicationService applicationService,
                            IAccountService accountService)
        {
            _interviewService = interviewService;
            _feedbackService = feedbackService;
            _slotService = slotService;
            _applicationService = applicationService;
            _accountService = accountService;
        }

        public async Task<IActionResult> OnGet(int interviewId, int applicationId, int round)
        {
            string? token = HttpContext.Session.GetString("token");
            int? role = HttpContext.Session.GetInt32("Role");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/Auth/Login");
            }
            if (role != (int)AccountRole.Interviewer)
            {
                return RedirectToPage("/Home");
            }
            var interview = await _interviewService.GetModelAsync(path: StoredURI.Interviews + $"/Nonfeedbacks/InterviewerId/{interviewId}/ApplicationId/{applicationId}/Round/{round}", token: token);
            if(interview == null)
            {
                return RedirectToPage("../Interviews/HomePageInterviewer");
            }
            var slot = await _slotService.GetModelAsync(path: StoredURI.Slot + $"/{interview.SlotId}", token: token);
            if (slot == null)
            {
                return RedirectToPage("../Interviews/HomePageInterviewer");
            }
            var application = await _applicationService.GetModelAsync(path: StoredURI.Application + $"/Get/Id/{interview.ApplicationId}", token: token);
            if (application == null)
            {
                return RedirectToPage("../Interviews/HomePageInterviewer");
            }
            var applicant = await _accountService.GetModelAsync(path: StoredURI.Account + $"/{application.ApplicantId}", token: token);
            if (applicant == null)
            {
                return RedirectToPage("../Interviews/HomePageInterviewer");
            }
            Interview = new ClientRepository.Models.InterviewFeedbackModel()
            {
                ApplicantName = applicant.FirstName + " " + applicant.LastName,
                ApplicationId = interview.ApplicationId,
                SlotName = slot.StartTime.ToString() + " / " + slot.EndTime.ToString(),
                Point = interview.Point,
                Round = interview.Round,
                Feedback = interview.Feedback,
            };
            return Page();
        }


        [BindProperty]
        public ClientRepository.Models.InterviewFeedbackModel Interview { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            string? token = HttpContext.Session.GetString("token");
            int? role = HttpContext.Session.GetInt32("Role");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/Auth/Login");
            }
            if (role != (int)AccountRole.Interviewer)
            {
                return RedirectToPage("/Home");
            }
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToPage("../Auth/Login");
            }
            Interview.InterviewerId = userId.Value;
            if (!ModelState.IsValid)
            {
                await OnGet(Interview.InterviewerId, Interview.ApplicationId, Interview.Round);
                return Page();
            }
            bool result = await _feedbackService.Update(Interview, path: StoredURI.Interviews + "/feedback", token: token);
            if(result)
            {
                return RedirectToPage("../Interviews/HomePageInterviewer");
            }
            await OnGet(Interview.InterviewerId, Interview.ApplicationId, Interview.Round);
            return Page();
        }
    }
}
