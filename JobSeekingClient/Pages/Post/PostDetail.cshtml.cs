using ClientRepository;
using ClientRepository.Models;
using ClientRepository.Service;
using ClientRepository.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace JobSeekingClient.Pages
{
    public class PostDetailModel : PageModel
    {
        public PostDTO Post { get; set; }

        private readonly IConfiguration config;

        private readonly IPostService postService;

        private readonly IApplicationService applicationService;
        private readonly ICategoryService _categoryService;
        private readonly ILevelService _levelService;
        private readonly ILocationService _locationService;
        private readonly IPostSkillService _postSkillService;
        private readonly IUserSkillService _userSkillService;
        public string Category { get; set; }

        public string Location { get; set; }

        public string Level { get; set; }

        public bool CanApply { get; set; } = true;

        public ApplicationModel OldApplication { get; set; }

        public IEnumerable<PostSkillModel> PostSkills { get; set; } 

        public PostDetailModel(IConfiguration config, IPostService postService, IApplicationService applicationService, 
            ICategoryService categoryService, ILevelService levelService, ILocationService locationService, IPostSkillService postSkillService,
            IUserSkillService userSkillService)
        {
            this.config = config;
            this.postService = postService;
            this.applicationService = applicationService;
            this._categoryService = categoryService;
            _levelService = levelService;
            _locationService = locationService;
            _postSkillService = postSkillService;
            _userSkillService = userSkillService;
        }
        public async Task<IActionResult> OnGet(int id)
        {
            string? token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("../Auth/Login");
            }
            int? role = HttpContext.Session.GetInt32("Role");
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToPage("../Auth/Login");
            }
            Post = postService.GetModelAsync(id).Result;
            if(Post != null)
            {
                var category = await _categoryService.GetModelAsync(path: StoredURI.Category + $"/{Post.CategoryId}", token: token);
                var level = await _levelService.GetModelAsync(path: StoredURI.Level + $"/{Post.LevelId}", token: token);
                var location = await _locationService.GetModelAsync(path: StoredURI.Location + $"/{Post.LocationId}", token: token);
                if (category == null || level == null || location == null)
                {
                    return RedirectToPage("../Home");
                }
                Category = category.Name;
                Location = location.Name;
                Level = level.Name;
                PostSkills = await _postSkillService.GetListAsync(path: StoredURI.PostSkill + $"/{id}", token: token);
                if (PostSkills == null || PostSkills.Count() == 0)
                {
                    ViewData["Error"] = "Post has no skills to apply!";
                    return Page();
                }
                if (Post.EndDate < DateTime.Now)
                {
                    CanApply = false;
                }
            }
            var applications = await applicationService.GetListAsync(path: StoredURI.Application + $"/ApplicationNonInterview/ApplicantId/{userId}", token: token);
            if(applications != null && applications.Count() == 2)
            {
                bool alreadyApplyPost = false;
                foreach(var application in applications)
                {
                    if(application.PostId == id)
                    {
                        alreadyApplyPost = true;
                    }
                }
                if (!alreadyApplyPost)
                {
                    CanApply = false;
                }
            }
            if (role != (int)AccountRole.Applicant)
            {
                CanApply = false;
            }
            applications = await applicationService.GetListAsync(path: StoredURI.Application + $"/Get/ApplicantId/{userId}/PostId/{id}", token: token);
            if (applications != null)
            {
                if (applications.Count > 0)
                {
                    foreach (var application in applications)
                    {
                        if (OldApplication == null)
                        {
                            OldApplication = application;
                        }
                       
                        if (application.Status != null)
                        {
                            CanApply = false;
                            return Page();
                        }
                    }
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostApply(int id, IFormFile cvFile)
        {
            string? token = HttpContext.Session.GetString("token");
            int? role = HttpContext.Session.GetInt32("Role");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("../Auth/Login");
            }
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToPage("../Auth/Login");
            }
            if (role != (int)AccountRole.Applicant)
            {
                ViewData["Error"] = "Need to be applicant account to apply cv!";
                await OnGet(id);
                return Page();
            }
            if (cvFile == null)
            {
                ViewData["Error"] = "CV file not located!";
                await OnGet(id);
                return Page();
            }
            if(!cvFile.FileName.ToLower().Contains(".pdf"))
            {
                ViewData["Error"] = "CV file must be pdf extension!";
                await OnGet(id);
                return Page();
            }
            var postSkillModels = await _postSkillService.GetListAsync(path: StoredURI.PostSkill + $"/{id}", token: token);
            if(postSkillModels == null || postSkillModels.Count() == 0)
            {
                ViewData["Error"] = "Post has no skills to apply!";
                await OnGet(id);
                return Page();
            }
            var userSkills = await _userSkillService.GetListAsync(path: StoredURI.UserSkill + $"/{userId}", token: token);
            if (userSkills == null || userSkills.Count == 0)
            {
                ViewData["Error"] = "You have no skills required!";
                await OnGet(id);
                return Page();
            }
            bool userContainSkill = false;
            foreach(var postSkill in postSkillModels)
            {
                foreach (var userSkill in userSkills)
                {
                    if (postSkill.SkillId == userSkill.SkillId)
                    {
                        userContainSkill = true;
                        break;
                    }
                }
            }
            if(!userContainSkill)
            {
                ViewData["Error"] = "You don't have post skills required!";
                await OnGet(id);
                return Page();
            }
            var applications = await applicationService.GetListAsync(path: StoredURI.Application + $"/Get/ApplicantId/{userId}/PostId/{id}", token: token);
            if (applications != null)
            {
                if (applications.Count > 0)
                {
                    foreach (var application in applications)
                    {
                        if(application.Status != null)
                        {
                            ViewData["Error"] = "You already interviewed this post!";
                            await OnGet(id);
                            return Page();
                        }
                        application.IsDeleted = true;
                        var res = await applicationService.Update(application, path: StoredURI.Application + $"/{application.Id}", token: token);
                        if (!res)
                        {
                            ViewData["Error"] = "Apply CV Error!";
                            await OnGet(id);
                            return Page();
                        }
                    }
                }
            }
            bool result = await applicationService.Create(id, userId.Value, cvFile, token);
            if(result)
            {
                ViewData["Success"] = "Apply CV Successful.";
                await OnGet(id);
                return Page();
            }
            ViewData["Error"] = "Apply CV Error!";
            await OnGet(id);
            return Page();
        }
    }
}
