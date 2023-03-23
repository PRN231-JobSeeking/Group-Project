using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppCore;
using AppCore.Models;
using ClientRepository.Service;
using ClientRepository.Models;
using ClientRepository.Utils;
using ClientRepository;
using ClientRepository.Service.Implementation;

namespace JobSeekingClient.Pages.Levels
{
    public class EditModel : PageModel
    {
        private readonly ILevelService _levelService;
        private readonly IPostService _postService;

        public EditModel(ILevelService levelService, IPostService postService)
        {
            _levelService = levelService;
            _postService = postService;
        }

        [BindProperty]
        public LevelModel Level { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            string? token = HttpContext.Session.GetString("token");
            int? role = HttpContext.Session.GetInt32("Role");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("../Auth/Login");
            }
            if (role != (int)AccountRole.Administrator)
            {
                return RedirectToPage("../Home");
            }
            string path = StoredURI.Level + "/" + id;
            var find = await _levelService.GetModelAsync(path: path, expression: c => c.IsDeleted == false, token: token);
            if (find == null)
            {
                return NotFound();
            }
            Level = find;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            string? token = HttpContext.Session.GetString("token");
            int? role = HttpContext.Session.GetInt32("Role");
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (role != (int)AccountRole.Administrator)
            {
                return RedirectToPage("../Home");
            }
            var levels = await _levelService.GetListAsync(expression: c => c.Name.ToLower().Equals(Level.Name.ToLower()), path: StoredURI.Level, token: token);
            if (levels != null && levels.Count > 0)
            {
                foreach (var level in levels)
                {
                    if (level.Id != Level.Id && level.IsDeleted == false)
                    {
                        ViewData["Error"] = "Already exist level name!";
                        await OnGetAsync(Level.Id);
                        return Page();
                    }
                }
            }
            var post = await _postService.GetListAsync(path: StoredURI.Post + "/GetAll", expression: c => c.IsDeleted == false && c.LevelId == Level.Id, token: token);
            if (post.Count > 0)
            {
                ViewData["Error"] = "Some post have this level";
                await OnGetAsync(Level.Id);
                return Page();
            }
            await _levelService.Update(Level, path: StoredURI.Level + "/" + Level.Id.ToString(), token: token);
            return RedirectToPage("./Index");
        }
    }
}
