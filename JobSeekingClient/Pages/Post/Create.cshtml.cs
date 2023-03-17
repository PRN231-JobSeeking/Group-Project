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

        public CreateModel(IPostService postService, ICategoryService categoryService, ILevelService levelService, ILocationService locationService)
        {
            _postService = postService;
            _categoryService = categoryService;
            _levelService = levelService;
            _locationService = locationService;
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
            return Page();
        }

        [BindProperty]
        public PostDTO Post { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
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
            if (!ModelState.IsValid)
            {
                return Page();
            }
            bool result = await _postService.Add(Post, path: StoredURI.Post, token: token);
            if(result)
            {
                return RedirectToPage("../Home");
            }
            ViewData["Error"] = "Post Add Error!";
            return Page();
        }
    }
}
