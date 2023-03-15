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

        public string Category { get; set; }

        public string Location { get; set; }

        public string Level { get; set; }

        public PostDetailModel(IConfiguration config, IPostService postService, IApplicationService applicationService, 
            ICategoryService categoryService, ILevelService levelService, ILocationService locationService)
        {
            this.config = config;
            this.postService = postService;
            this.applicationService = applicationService;
            this._categoryService = categoryService;
            _levelService = levelService;
            _locationService = locationService;
        }
        public async Task<IActionResult> OnGet(int id)
        {
            string? token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
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
            if (role != (int)AccountRole.Applicant)
            {
                ViewData["Error"] = "Need to be applicant account to apply cv!";
                return Page();
            }
            if (cvFile == null)
            {
                ViewData["Error"] = "CV file not located!";
                return Page();
            }
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToPage("../Auth/Login");
            }
            var applications = await applicationService.GetListAsync(path: StoredURI.Application + $"/Get/ApplicantId/{userId}/PostId/{id}", token: token);
            if (applications != null)
            {
                if (applications.Count > 0)
                {
                    foreach (var application in applications)
                    {
                        application.Status = false;
                        application.IsDeleted = true;
                        var res = await applicationService.Update(application, path: StoredURI.Application + $"/{application.Id}", token: token);
                        if (!res)
                        {
                            ViewData["Error"] = "Apply CV Error!";
                            return Page();
                        }
                    }
                }
            }
            Post = postService.GetModelAsync(id).Result;
            if (Post != null)
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
            }
            bool result = await applicationService.Create(id, userId.Value, cvFile, token);
            if(result)
            {
                ViewData["Success"] = "Apply CV Successful.";
                return Page();
            }
            ViewData["Error"] = "Apply CV Error!";
            return Page();
        }
    }
}
