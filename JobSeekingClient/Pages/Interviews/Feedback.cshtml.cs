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
        private readonly IFeedbackService _feedbackService;

        public FeedbackModel(IInterviewService interviewService, IFeedbackService feedbackService)
        {
            _interviewService = interviewService;
            _feedbackService = feedbackService;
        }

        public async Task<IActionResult> OnGet(int interviewId, int applicantId)
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
            var interview = await _interviewService.GetModelAsync(path: StoredURI.Interviews + $"/Nonfeedbacks/InterviewerId/{interviewId}/ApplicantId/{applicantId}", token: token);
            if(interview == null)
            {
                return RedirectToPage("../Interviews/HomePageInterviewer");
            }
            Interview = new ClientRepository.Models.InterviewFeedbackModel()
            {
                ApplicationId = interview.ApplicationId,
                SlotId = interview.SlotId,
                InterviewerId = interview.InterviewerId,
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
            if (!ModelState.IsValid)
            {
                return Page();
            }
            bool result = await _feedbackService.Update(Interview, path: StoredURI.Interviews + "/feedback", token: token);
            if(result)
            {
                return RedirectToPage("../Interviews/HomePageInterviewer");
            }
            return Page();
        }
    }
}
