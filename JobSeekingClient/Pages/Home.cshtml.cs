using ClientRepository;
using ClientRepository.Models;
using ClientRepository.Service;
using ClientRepository.Service.Implementation;
using JobSeekingClient.Pages.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NuGet.Common;
using System.Collections.Generic;
using System.Diagnostics;

namespace JobSeekingClient.Pages.Home
{
    public class HomeModel : PageModel
    {
        private readonly IPostService _postService;
        private readonly IApplicationService _applicationService;
        private readonly IInterviewService _interviewService;
        private readonly ISlotService _slotService;

        public HomeModel(IPostService postService, IApplicationService applicationService,
            IInterviewService interviewService, ISlotService slotService)
        {
            _postService = postService;
            _applicationService = applicationService;
            _interviewService = interviewService;
            _slotService = slotService;
        }

        //used to show the list of application ready to be assigned interviews for HR
        [BindProperty]
        public IList<ApplicationModel> _applications2 { get; set; }

        //used to show the list of applications ready to be approved or deny for HR
        [BindProperty]
        public IList<ApplicationModel> _applications { get; set; }

        //used to show the list of interviews ready to be reviewed
        [BindProperty]
        public IList<InterviewModel> _interviews { get; set; }

        [BindProperty]
        public bool isWarning { get; set; }

        public IActionResult OnGet()
        {
            //user for role checking
            var roleId = HttpContext.Session.GetInt32("Role");

            //user id for interview/application/post tracking
            var userId = HttpContext.Session.GetInt32("UserId");

            if (roleId == null)
            {
                return RedirectToPage("/Auth/Login");
            }

            //redirect appropriately.
            //1.Admin
            //2.HR
            //3.Internviewer
            //4.Applicants
            switch (roleId)
            {
                //Admin
                case 1:
                    {
                        return Page();
                        break;
                    }

                //HR
                //shows warning should there be applicants whose interview are not assigned
                case 2:
                    {
                        var token = HttpContext.Session.GetString("token");
                        var _applicationList = _applicationService.GetListAsync(path: StoredURI.Application, expression: c => c.IsDeleted == false, param: null, token: token).Result.ToList();
                        var _interviewList = _interviewService.GetListAsync(path: StoredURI.Interviews, expression: null, param: null, token: null).Result.ToList();

                        Debug.WriteLine("Home.OnGet: _applicationList count: " + _applicationList.Count);
                        Debug.WriteLine("Home.OnGet: _interviewList count: " + _interviewList.Count);

                        _applications = new List<ApplicationModel>();
                        _applications2 = new List<ApplicationModel>();
                        _interviews = new List<InterviewModel>();

                        //get list to check whether HR has application that needs attention
                        foreach (ApplicationModel item in _applicationList)
                        {
                            //check application is not deleted
                            //status == null: application has not yet been reviewed
                            if (item.IsDeleted == false && item.Status == null)
                            {
                                Debug.WriteLine("Home.OnGet: Checking application id: " + item.Id);

                                int count = 0;
                                int count2 = 0;

                                //used to check if interview for application is completed
                                foreach (InterviewModel item2 in _interviewList)
                                {

                                    //counts the amount of in-compltete interview
                                    if (item2.IsDeleted == false
                                        && item2.ApplicationId == item.Id
                                        && item2.Feedback == null)
                                    {
                                        Debug.WriteLine("Home.OnGet: application id: " + item.Id + " has interview id " + item2.InterviewerId);
                                        count++;
                                    }

                                    //counts the amount of complteted interview
                                    if (item2.IsDeleted == false
                                        && item2.ApplicationId == item.Id
                                        && item2.Feedback != null)
                                    {
                                        Debug.WriteLine("Home.OnGet: application id: " + item.Id + " has interview id " + item2.InterviewerId + " with feedback");
                                        count++;
                                        count2++;
                                    }

                                }

                                //only application without 2 interviews completed will be added to list1
                                if (count < 2)
                                {
                                    Debug.WriteLine("Home.OnGet: application id: " + item.Id + " doesn't have enough interviews");
                                    _applications.Add(item);
                                }
                                //application with 2 completed interviews but no approval will be added to list2
                                else if (count2 > 1)
                                {
                                    if (item.Status == null)
                                    {
                                        Debug.WriteLine("Home.OnGet: application id: " + item.Id + " has enough interviews, but not enough feedback");
                                        _applications2.Add(item);
                                    }
                                }
                            }
                        }

                        Debug.WriteLine("Home.OnGet: applcation that needs interview :" + _applications.Count);
                        Debug.WriteLine("Home.OnGet: applcation that needs review :" + _applications2.Count);

                        //if list has 1 or more than 1 incoming application, show warning and list
                        if (_applications.Count == 0 && _applications2.Count == 0)
                        {
                            return RedirectToPage("/Applications/Index");
                        }

                        if (_applications.Count != 0)
                            ViewData["message1"] = "You have " + _applications.Count + " incoming application that needs interviews!";
                        if (_applications2.Count != 0)
                            ViewData["message2"] = "You have " + _applications2.Count + " incoming application that needs reviews!";
                        return Page();

                        break;
                    }

                //Interviewer
                //shows warning should there be incoming interview for logged in interviewer
                case 3:
                    {
                        _interviews = new List<InterviewModel>();

                        var _interviewList = _interviewService.GetListAsync(path: StoredURI.Interviews, expression: c => c.InterviewerId == userId && c.IsDeleted == false, param: null, token: null).Result.ToList();

                        Debug.WriteLine("Home.OnGet: Getting user's interviews :" + _interviewList.Count());
                        //get list to check whether interviewer has incoming interview
                        foreach (InterviewModel item in _interviewList)
                        {
                            if (item.Feedback == null)
                            {
                                var slotPath = StoredURI.Slot + "/" + item.SlotId;
                                item.Slot = _slotService.GetModelAsync(path: slotPath, param: null, token: null).Result;
                                Debug.WriteLine("Home.OnGet: Pending interview found :" + item.ApplicationId);
                                _interviews.Add(item);
                            }
                        }

                        //if list has 1 or more than 1 incoming interview, show warning and list
                        if (_interviews.Count != 0)
                        {
                            ViewData["message3"] = "You have " + _interviews.Count + " incoming interview!";
                            return Page();
                        }
                        //else return the list of interviews that the interviewer/applicant as conducted
                        else
                        {
                            _interviews = _interviewList;
                            return Page();
                        }
                        break;
                    }
                //Applicants
                case 4:
                    {
                        _interviews = new List<InterviewModel>();

                        var _interviewList = _interviewService.GetListAsync(path: StoredURI.Interviews, expression: null, param: null, token: null).Result.ToList();
                        //get list to check whether interviewer has incoming interview
                        foreach (InterviewModel item in _interviewList)
                        {
                            var application = _applicationService.GetModelAsync(path: StoredURI.Application + "/Get/Id/" + item.ApplicationId).Result;
                            Debug.WriteLine("Home.OnGet: Found applicant application :" + application.Id);
                            if (application.ApplicantId == userId && item.IsDeleted == false && item.Feedback == null)
                            {
                                var slotPath = StoredURI.Slot + "/" + item.SlotId;
                                item.Slot = _slotService.GetModelAsync(path: slotPath, param: null, token: null).Result;
                                _interviews.Add(item);
                            }
                        }

                        //if list has 1 or more than 1 incoming interview, show warning and list
                        //else, return the complete list of interviews
                        if (_interviews.Count != 0)
                        {
                            ViewData["message3"] = "You have " + _interviews.Count + " incoming interview!";
                        }
                        else
                        {
                            _interviews = _interviewList;
                        }

                        //checking for applications
                        var _applicationList = _applicationService.GetListAsync(path: StoredURI.Application, expression: c => c.IsDeleted == false && c.ApplicantId == userId, param: null, token: null).Result.ToList();
                        Debug.WriteLine("Home.OnGet: Found applicant application list :" + _applicationList.Count() + " of user id:" + userId);
                        _applications = new List<ApplicationModel>();
                        _applications2 = new List<ApplicationModel>();
                        //warn user should they have pending application
                        foreach (var item in _applicationList)
                        {
                            if (item.Status == null)
                            {
                                _applications.Add(item);
                            }
                        }
                        if (_applications.Count != 0)
                        {
                            Debug.WriteLine("Home.OnGet: Found pending application :" + _applications.Count());
                            ViewData["message1"] = "You have " + _applications.Count + " pending application!";
                        }
                        else
                        {
                            // if there's no pending, show the user their failed and passed applications
                            foreach (var item in _applicationList)
                            {
                                if ((bool)item.Status)
                                {
                                    _applications.Add(item);
                                }
                            }

                            foreach (var item in _applicationList)
                            {
                                if ((bool)item.Status)
                                {
                                    _applications2.Add(item);
                                }
                            }
                        }
                        return Page();
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            return Page();
        }
    }
}
