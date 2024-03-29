using ClientRepository;
using ClientRepository.Models;
using ClientRepository.Service;
using ClientRepository.Service.Implementation;
using ClientRepository.Utils;
using JobSeekingClient.Pages.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
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
        private readonly ILocationService _locationService;
        private readonly ILevelService _levelService;
        private readonly ICategoryService _categoryService;
        private readonly ISkillService _skillService;
        private readonly IPostSkillService _postSkillService;

        public HomeModel(IPostService postService, IApplicationService applicationService,
            IInterviewService interviewService, ISlotService slotService, ILocationService locationService,
            ILevelService levelService, ICategoryService categoryService, ISkillService skillService
            , IPostSkillService postSkillService)
        {
            _postService = postService;
            _applicationService = applicationService;
            _interviewService = interviewService;
            _slotService = slotService;
            _locationService = locationService;
            _levelService = levelService;
            _categoryService = categoryService;
            _skillService = skillService;
            _postSkillService = postSkillService;
        }
        //used to show level
        [BindProperty]
        public IList<LevelModel> _level { get; set; }

        //used to show category
        [BindProperty]
        public IList<CategoryModel> _category { get; set; }

        //used to show skill
        [BindProperty]
        public IList<Skill> _skill { get; set; }

        //used to show the list of post for default home view
        [BindProperty]
        public IList<PostDTO> _post { get; set; }

        //used to show the list of application ready to be assigned interviews for HR
        [BindProperty]
        public IList<ApplicationModel> _applications2 { get; set; }

        //used to show the list of applications ready to be approved or deny for HR
        [BindProperty]
        public IList<ApplicationModel> _applications { get; set; }

        //used to show the list of interviews ready to be reviewed
        [BindProperty]
        public IList<InterviewModel> _interviews { get; set; }

        //used for searching and filtering function
        [BindProperty]
        public string _searchInput { get; set; }



        //used for location filtering
        [BindProperty]
        public string _locationFilterInput { get; set; }
        [BindProperty]
        public string _categoryFilterInput { get; set; }
        [BindProperty]
        public string _skillFilterInput { get; set; }
        [BindProperty]
        public string _levelFilterInput { get; set; }
        [BindProperty]
        public IList<LocationModel> _locations { get; set; }
        [BindProperty]
        public string newMode { get; set; }
        [BindProperty]
        public int _isNewsMode { get; set; }

        //used to show the player news
        [BindProperty]
        public bool isNews { get; set; }

        public IActionResult OnGet(string? _locationFilterInput, string? _categoryFilterInput,
            string? _skillFilterInput, string? newMode, string? _searchInput, string? _levelFilterInput)
        {
            _isNewsMode = 0;

            //user for role checking
            var roleId = HttpContext.Session.GetInt32("Role");

            //token for API
            var token = HttpContext.Session.GetString("token");

            //if user is not logged in, back to login
            if (roleId == null)
            {
                return RedirectToPage("/Auth/Login");
            } else if(roleId==(int)AccountRole.Administrator)
            {
                return RedirectToPage("/Accounts/IndexAdmin");
            }
            
            int _newsMode = 0;

            if (int.TryParse(newMode, out _newsMode))
            {
                Debug.WriteLine("Home.OnGet: News mode is now set to " + _isNewsMode);

                _isNewsMode = _newsMode;
                if (_isNewsMode == 1 || _isNewsMode == 2)
                {
                    Debug.WriteLine("Home.OnGet: News mode is on");
                    isNews = true;
                }
                else
                {
                    Debug.WriteLine("Home.OnGet: News mode is off");
                    isNews = false;
                }
            }
            else
            {
                Debug.WriteLine("Home.OnGet: News mode is off");
                isNews = false;
            }


            //initialize basic vars
            _locations = _locationService.GetListAsync(path: StoredURI.Location, token: token).Result.ToList();
            Debug.WriteLine("Home.OnGet: _locations count: " + _locations.Count);
            _level = _levelService.GetListAsync(path: StoredURI.Level, token: token).Result.ToList();
            Debug.WriteLine("Home.OnGet: _levelService count: " + _level.Count);
            _category = _categoryService.GetListAsync(path: StoredURI.Category, token: token).Result.ToList();
            Debug.WriteLine("Home.OnGet: _category count: " + _category.Count);
            _skill = _skillService.GetListAsync(path: StoredURI.Skill, token: token).Result.ToList();
            Debug.WriteLine("Home.OnGet: _skill count: " + _skill.Count);



            if (!isNews)
            {
                _post = _postService.GetListAsync(path: StoredURI.Post + "/GetAll", expression: c => c.IsDeleted == false && c.Amount >= 1 && c.Status == true, param: null, token: token).Result.ToList();

                Debug.WriteLine("Home.OnGet: Getting post list. Count: " + _post.Count);

                //if search field is not empty, start searching
                if (_searchInput != null)
                {
                    if (!_searchInput.Equals(""))
                    {
                        _post = _post.Where(x => x.Title.IndexOf(_searchInput, StringComparison.OrdinalIgnoreCase) >= 0 || x.Description.Contains(_searchInput)).ToList();
                        Debug.WriteLine("Home.OnGet: search input detected. Remaining _post: " + _post.Count);
                    }
                }

                //if location is not set to all, start searching
                if (_locationFilterInput != null)
                {
                    if (!_locationFilterInput.Equals("null"))
                    {
                        _post = _post.Where(x => x.LocationId == int.Parse(_locationFilterInput)).ToList();
                        Debug.WriteLine("Home.OnGet: location search input detected. Remaining _post: " + _post.Count);
                    }
                }


                //if category is not set to all, start searching
                if (_categoryFilterInput != null)
                {
                    if (!_categoryFilterInput.Equals("null"))
                    {
                        _post = _post.Where(x => x.CategoryId == int.Parse(_categoryFilterInput)).ToList();
                        Debug.WriteLine("Home.OnGet: category input detected. Remaining _post: " + _post.Count);
                    }
                }
                //if skill is not set to all, start searching
                int _skillInput = 0;

                if (_skillFilterInput != null)
                {
                    if (!_skillFilterInput.Equals("null"))
                    {
                        Debug.WriteLine("Home.OnGet: skill search input detected. Remaining _post: " + _post.Count);

                        if (int.TryParse(_skillFilterInput, out _skillInput))
                        {
                            var _newPost = new List<PostDTO>();

                            foreach (var item in _post)
                            {
                                var PostSkills = _postSkillService.GetListAsync(path: StoredURI.PostSkill + "/" + item.Id, token: token).Result.ToList();

                                Debug.WriteLine("Home.OnGet: Finding skill for " + item.Id + " with skill " + _skillInput);
                                Debug.WriteLine("Home.OnGet: Found " + PostSkills.Count() + " skills related to " + item.Id);

                                var skill = PostSkills.FirstOrDefault(x => x.PostId == item.Id && x.SkillId == _skillInput);

                                if (skill != null)
                                {
                                    _newPost.Add(item);
                                }

                            }

                            _post = _newPost;
                        }
                        else
                        {
                            Debug.WriteLine("Home.OnGet: Skill validation failed for input: " + _skillFilterInput);
                        }

                    }
                }
                //if skill is not set to all, start searching
                if (_levelFilterInput != null)
                {
                    if (!_levelFilterInput.Equals("null"))
                    {
                        _post = _post.Where(x => x.LevelId == int.Parse(_levelFilterInput)).ToList();
                        Debug.WriteLine("Home.OnGet: level search input dectected. Remaining _post: " + _post.Count);
                    }
                }
            }
            else
            {
                _post = _postService.GetListAsync(path: StoredURI.Post + "/GetAll", expression: c => c.IsDeleted == false, param: null, token: token).Result.ToList();
            }


            //user id for interview/application/post tracking
            var userId = HttpContext.Session.GetInt32("UserId");

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
                        var _applicationList = _applicationService.GetListAsync(path: StoredURI.Application, expression: c => c.IsDeleted == false, param: null, token: token).Result.ToList();
                        var _interviewList = _interviewService.GetListAsync(path: StoredURI.Interviews, expression: null, param: null, token: token).Result.ToList();

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
                                        var _applcationInterview = _applicationList.FirstOrDefault(x => x.Id == item2.ApplicationId);
                                        if (_applcationInterview.Status == null)
                                        {
                                            Debug.WriteLine("Home.OnGet: application id: " + item.Id + " has interview id " + item2.InterviewerId);
                                        }
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
                                        Debug.WriteLine("Home.OnGet: application id: " + item.Id + " has enough interviews, but no feedback");
                                        _applications2.Add(item);
                                    }
                                }
                            }
                        }

                        Debug.WriteLine("Home.OnGet: applcation that needs interview :" + _applications.Count);
                        Debug.WriteLine("Home.OnGet: applcation that needs review :" + _applications2.Count);

                        //based on news mode, show the appropriate message
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

                        var _interviewList = _interviewService.GetListAsync(path: StoredURI.Interviews, expression: c => c.InterviewerId == userId && c.IsDeleted == false, param: null, token: token).Result.ToList();
                        var _applicationList = _applicationService.GetListAsync(path: StoredURI.Application, expression: c => c.IsDeleted == false, param: null, token: token).Result.ToList();

                        Debug.WriteLine("Home.OnGet: Getting user's interviews :" + _interviewList.Count());
                        //get list to check whether interviewer has incoming interview
                        foreach (InterviewModel item in _interviewList)
                        {
                            if (item.Feedback == null)
                            {
                                var slotPath = StoredURI.Slot + "/" + item.SlotId;
                                item.Slot = _slotService.GetModelAsync(path: slotPath, param: null, token: token).Result;
                                {
                                    var _applcationInterview = _applicationList.FirstOrDefault(x => x.Id == item.ApplicationId);
                                    if (_applcationInterview.Status == null)
                                    {
                                        Debug.WriteLine("Home.OnGet: Pending interview found :" + item.ApplicationId);
                                        _interviews.Add(item);
                                    }
                                }
                            }
                        }

                        //if list has 1 or more than 1 incoming interview, show warning and list
                        if (_interviews.Count != 0)
                        {
                            ViewData["message1"] = "You have " + _interviews.Count + " incoming interview!";
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

                        //checking for applications
                        var _applicationList = _applicationService.GetListAsync(path: StoredURI.Application, expression: c => c.IsDeleted == false && c.ApplicantId == userId, param: null, token: token).Result.ToList();
                        Debug.WriteLine("Home.OnGet: Found applicant application list :" + _applicationList.Count() + " of user id:" + userId);


                        var _tempInterviewList = _interviewService.GetListAsync(path: StoredURI.Interviews, expression: e => e.IsDeleted == false, param: null, token: token).Result.ToList();

                        var _interviewList = new List<InterviewModel>();
                        foreach (var item in _tempInterviewList)
                        {
                            foreach (var item2 in _applicationList)
                            {
                                if (item.ApplicationId == item2.Id)
                                {
                                    _interviewList.Add(item);
                                }
                            }
                        }

                        //get list to check whether interviewer has incoming interview
                        foreach (InterviewModel item in _interviewList)
                        {
                            var application = _applicationService.GetModelAsync(path: StoredURI.Application + "/Get/Id/" + item.ApplicationId, token: token).Result;
                            Debug.WriteLine("Home.OnGet: Found applicant application :" + application.Id);
                            if (application.ApplicantId == userId && item.IsDeleted == false && item.Feedback == null && DateTime.Compare(DateTime.Now, item.Date) < 0)
                            {
                                var slotPath = StoredURI.Slot + "/" + item.SlotId;
                                item.Slot = _slotService.GetModelAsync(path: slotPath, param: null, token: token).Result;
                                var _applcationInterview = _applicationList.FirstOrDefault(x => x.Id == item.ApplicationId);
                                if (_applcationInterview.Status == null)
                                {
                                    Debug.WriteLine("Home.OnGet: Pending interview found :" + item.ApplicationId);
                                    _interviews.Add(item);
                                }
                            }

                        }

                        //if list has 1 or more than 1 incoming interview, show warning and list
                        //else, return the complete list of interviews
                        if (_interviews.Count != 0)
                        {
                            ViewData["message1"] = "You have " + _interviews.Count + " incoming interview!";
                        }
                        else
                        {
                            _interviews = _interviewList;
                        }

                        Debug.WriteLine("Home.OnGet: Found applicant application list :" + _applicationList.Count() + " of user id:" + userId);
                        _applications = new List<ApplicationModel>();
                        _applications2 = new List<ApplicationModel>();
                        var _applications3 = new List<ApplicationModel>();
                        //warn user should they have pending application
                        foreach (var item in _applicationList)
                        {
                            if (item.Status == null)
                            {
                                _applications.Add(item);
                            }
                        }

                        Debug.WriteLine("Home.OnGet: Found pending application :" + _applications.Count());
                        if (_applications.Count() >= 1)
                        {
                            ViewData["message2"] = "You have " + _applications.Count + " pending application!";
                        }

                        // if there's no pending, show the user their failed and passed applications
                        foreach (var item in _applicationList)
                        {
                            if (item.Status != null)
                            {
                                if (item.Status == true)
                                {
                                    _applications2.Add(item);
                                }
                            }

                        }

                        if (_applications2.Count != 0)
                        {
                            Debug.WriteLine("Home.OnGet: Found passed application :" + _applications2.Count);
                            ViewData["message3"] = "You have reviewed applications!";
                            ViewData["message4"] = "You have " + _applications2.Count + " passed applications!";
                        }

                        foreach (var item in _applicationList)
                        {
                            if (item.Status != null)
                            {
                                if ((bool)item.Status == false)
                                {
                                    _applications3.Add(item);
                                }
                            }
                        }

                        if (_applications3.Count != 0)
                        {
                            Debug.WriteLine("Home.OnGet: Found failed application :" + _applications3.Count);
                            ViewData["message3"] = "You have reviewed applications!";
                            ViewData["message5"] = "You have " + _applications3.Count + " failed applications!";
                        }

                        if (_isNewsMode == 2)
                        {
                            _applications = _applications2;
                            _applications2 = _applications3;
                        }

                        Debug.WriteLine("Home.OnGet: Showing applicant's posts:" + _applications3.Count);

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
