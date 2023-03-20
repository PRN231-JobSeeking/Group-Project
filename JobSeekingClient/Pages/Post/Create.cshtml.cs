using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using AppCore;
using AppCore.Models;
using ClientRepository.Service;
using ClientRepository;
using ClientRepository.Utils;
using ClientRepository.Models;

namespace JobSeekingClient.Pages.Post
{
    public class CreateModel : PageModel
    {
        private readonly IPostService _postService;
        private readonly ICategoryService _categoryService;
        private readonly ILevelService _levelService;
        private readonly ILocationService _locationService;
        private readonly ISkillService _skillService;
        private readonly IPostSkillService _postSkillService;

        public CreateModel(IPostService postService, ICategoryService categoryService, ILevelService levelService, 
            ILocationService locationService, ISkillService skillService, IPostSkillService postSkillService)
        {
            _postService = postService;
            _categoryService = categoryService;
            _levelService = levelService;
            _locationService = locationService;
            _skillService = skillService;
            _postSkillService = postSkillService;
        }

        public async Task<IActionResult> OnGet()
        {
            string? token = HttpContext.Session.GetString("token");
            int? role = HttpContext.Session.GetInt32("Role");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("../Auth/Login");
            }
            if (role != (int)AccountRole.HR)
            {
                return RedirectToPage("../Home");
            }
            var categories = await _categoryService.GetListAsync(path: StoredURI.Category, token: token);
            if(categories == null || categories.Count() == 0)
            {
                return RedirectToPage("../Home");
            }
            var levels = await _levelService.GetListAsync(path: StoredURI.Level, token: token);
            if (levels == null || levels.Count() == 0)
            {
                return RedirectToPage("../Home");
            }
            var locations = await _locationService.GetListAsync(path: StoredURI.Location, token: token);
            if (locations == null || locations.Count() == 0)
            {
                return RedirectToPage("../Home");
            }
            ViewData["CategoryId"] = new SelectList(categories, "Id", "Name");
            ViewData["LevelId"] = new SelectList(levels, "Id", "Name");
            ViewData["LocationId"] = new SelectList(locations, "Id", "Name");
            Skills = PopulateSkills();
            return Page();
        }

        [BindProperty]
        public PostDTO Post { get; set; }

        public List<SelectListItem> Skills { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(string[] SkillIds)
        {
            string? token = HttpContext.Session.GetString("token");
            int? role = HttpContext.Session.GetInt32("Role");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("../Auth/Login");
            }
            if (role != (int)AccountRole.HR)
            {
                return RedirectToPage("../Home");
            }
            if(SkillIds == null || SkillIds.Length == 0)
            {
                ViewData["Error"] = "Choose at least one skill for post!";
                await OnGet();
                return Page();
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if(Post.Amount <= 0)
            {
                ViewData["Error"] = "Post Amount must more than 0!";
                await OnGet();
                return Page();
            }
            if(Post.StartDate.Date.CompareTo(DateTime.Now.Date) < 0)
            {
                ViewData["Error"] = "Start Date must more or equal current day!";
                await OnGet();
                return Page();
            }
            if(Post.StartDate.CompareTo(Post.EndDate) > 0)
            {
                ViewData["Error"] = "Start Date must less than End Date!";
                await OnGet();
                return Page();
            }
            if (Post.StartDate.Date.CompareTo(DateTime.Now.AddDays(10).Date) > 0)
            {
                ViewData["Error"] = "Start Date must less than 10 day from current day!";
                await OnGet();
                return Page();
            }
            if (Post.EndDate.Date.CompareTo(Post.StartDate.AddDays(3).Date) < 0)
            {
                ViewData["Error"] = "End Date must more than 3 day from start date!";
                await OnGet();
                return Page();
            }
            if (Post.EndDate.Date.CompareTo(Post.StartDate.AddMonths(2).Date) > 0)
            {
                ViewData["Error"] = "End Date must less than 2 month from start date!";
                await OnGet();
                return Page();
            }
            var posts = await _postService.GetListAsync(expression: p => p.Title.ToLower().Equals(Post.Title.ToLower()) && p.Status == true && p.IsDeleted == false,path: StoredURI.Post + "/GetAll", token: token);
            if(posts != null && posts.Count() > 0)
            {
                ViewData["Error"] = "Post Title already exist( you need to close post or complete all required amount to create this title)!";
                await OnGet();
                return Page();
            }
            bool result = await _postService.Add(Post, path: StoredURI.Post, token: token);
            if(result)
            {
                posts = await _postService.GetListAsync(expression: p => p.Status == Post.Status && p.StartDate.CompareTo(Post.StartDate) == 0
                                                                            && p.IsDeleted == false && p.Amount == Post.Amount
                                                                            && p.CategoryId == Post.CategoryId && p.CreateDate.CompareTo(Post.CreateDate) == 0
                                                                            && p.EndDate.CompareTo(Post.EndDate) == 0 && p.Description.Equals(Post.Description)
                                                                            && p.LevelId == Post.LevelId && p.LocationId == Post.LocationId
                                                                            && p.Title.Equals(Post.Title),path: StoredURI.Post + "/GetAll", token: token);
                PostDTO post = null;
                if(posts != null && posts.Count > 0)
                {
                    post = posts.FirstOrDefault();
                }
                if (post != null)
                {
                    foreach (string Id in SkillIds)
                    {
                        PostSkillModel postSkillRequired = new PostSkillModel()
                        {
                            PostId = post.Id,
                            SkillId = int.Parse(Id),
                            SkillName = "",
                        };
                        result = await _postSkillService.Add(postSkillRequired, path: StoredURI.PostSkill, token: token);
                        if(!result)
                        {
                            ViewData["Error"] = "Post Skill Add Error!";
                            await OnGet();
                            return Page();
                        }
                    }
                }
                return RedirectToPage("../Home");
            }
            ViewData["Error"] = "Post Add Error!";
            await OnGet();
            return Page();
        }

        private List<SelectListItem> PopulateSkills()
        {
            List<ClientRepository.Models.Skill> skills = _skillService.GetListAsync(path: StoredURI.Skill, expression: c => c.IsDeleted == false, param: null, token: null).Result;
            List<SelectListItem> SkillList = (from p in skills
                                              select new SelectListItem
                                              {
                                                  Text = p.Name,
                                                  Value = p.Id.ToString()
                                              }).ToList();

            return SkillList;
        }
    }
}
