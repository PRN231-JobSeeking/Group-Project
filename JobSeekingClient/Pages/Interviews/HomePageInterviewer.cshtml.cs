using AppCore.Models;
using ClientRepository;
using ClientRepository.Models;
using ClientRepository.Service;
using ClientRepository.Service.Implementation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace JobSeekingClient.Pages.Interviews
{
    public class HomePageInterviewerModel : PageModel
    {
        private readonly IInterviewService _interviewService;

        public HomePageInterviewerModel(IInterviewService interviewService)
        {
            _interviewService = interviewService;
        }

        public IList<InterviewModel> Interview = new List<InterviewModel>();

        public SelectList DateRange { get; set; }
        public string SelectedCategory { get; set; }

       

        private void PopulateList()
        {
            List<SelectListItem> categoryList = new List<SelectListItem>
                {
                    new SelectListItem { Value = "1", Text = "Need to grade" },
                    new SelectListItem { Value = "2", Text = "Interview today" },
                    new SelectListItem { Value = "3", Text = "Coming soon" }
                };
            DateRange = new SelectList(categoryList, "Value", "Text");
        }
        public async Task<IActionResult> OnGetAsync(string SelectedCategory)
        {
            string? token = HttpContext.Session.GetString("token");
            int? test = HttpContext.Session.GetInt32("Role");
            int? id = HttpContext.Session.GetInt32("UserId");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/Auth/Login");
            }
            if (test != 3)
            {
                return RedirectToPage("/Home");
            }         
            if(SelectedCategory != null)
            { 
                if(SelectedCategory.Equals("1"))
                {
                    var list = await _interviewService.GetListAsync(path: StoredURI.Interviews, expression: c => c.InterviewerId == id & c.IsDeleted == false & c.Point==0 & c.Date.CompareTo(DateTime.Now)<0, token: token);
                    if (list != null)
                    {
                        Interview = list;
                    }
                    PopulateList();
                    return Page();
                } else if (SelectedCategory.Equals("2")) 
                {
                    DateTime today= DateTime.Now;
                    var list = await _interviewService.GetListAsync(path: StoredURI.Interviews, expression: c => c.InterviewerId == id & c.IsDeleted == false & c.Date.Day == today.Day & c.Date.Year==today.Year & c.Date.Month==today.Month , token: token);
                    if (list != null)
                    {
                        Interview = list;
                    }
                    PopulateList();
                    return Page();
                }
                else if (SelectedCategory.Equals("3"))
                {
                    var list = await _interviewService.GetListAsync(path: StoredURI.Interviews, expression: c => c.InterviewerId == id & c.IsDeleted == false & c.Date.CompareTo(DateTime.Now) > 0, token: token);
                    if (list != null)
                    {
                        Interview = list;
                    }
                    PopulateList();
                    return Page();
                }
            }
            var listt = await _interviewService.GetListAsync(path: StoredURI.Interviews, expression: c => c.InterviewerId == id & c.IsDeleted == false, token: token);
            if (listt != null)
            {
                Interview = listt;
            }
            PopulateList();
            return Page();

        }
    }
}
