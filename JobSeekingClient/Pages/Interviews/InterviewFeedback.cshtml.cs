using AppCore.Models;
using ClientRepository;
using ClientRepository.Models;
using ClientRepository.Service;
using ClientRepository.Service.Implementation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace JobSeekingClient.Pages.Interviews
{
    public class InterviewFeedbackModel : PageModel
    {

        private readonly IInterviewService _interviewService;

        public InterviewFeedbackModel(IInterviewService interviewService)
        {
            _interviewService = interviewService;
        }

        [BindProperty]
        public InterviewModel _interviews { get; set; }


        public IActionResult OnGet(int applicationId, int round)
        {
            //user for role checking
            var roleId = HttpContext.Session.GetInt32("Role");

            //user id for interview/application/post tracking
            var userId = HttpContext.Session.GetInt32("UserId");

            //token for API
            var token = HttpContext.Session.GetString("token");

            if (roleId != 3)
            {
                return RedirectToPage("/Auth/Login");
            }

            string path3 = StoredURI.Interviews;
            var find3 = _interviewService.GetListAsync(token: token, path: path3).Result;
            if (find3 == null)
            {
                Debug.WriteLine("InterviewFeedbackModel.OnGet: Interview list not found");
                return NotFound();
            }

            var interview = find3.FirstOrDefault(p => p.ApplicationId == applicationId && p.IsDeleted == false && p.Round == round);

            if (interview == null)
            {
                Debug.WriteLine("InterviewFeedbackModel.OnGet: Interview not found");
                return NotFound();
            }

            _interviews = interview;

            return Page();

        }
    }
}
