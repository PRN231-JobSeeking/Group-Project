using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AppCore;
using AppCore.Models;
using ClientRepository.Service;
using ClientRepository.Models;
using ClientRepository;
using ClientRepository.Utils;

namespace JobSeekingClient.Pages.Levels
{
    public class DeleteModel : PageModel
    {
        private readonly ILevelService _levelService;
        private readonly IPostService _postService;

        public DeleteModel(ILevelService levelService, IPostService postService)
        {
            _levelService = levelService;
            _postService = postService;
        }

        [BindProperty]
      public LevelModel Level { get; set; }

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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            string path = StoredURI.Level + "/" + id;
            string? token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("../Auth/Login");
            }
            int? role = HttpContext.Session.GetInt32("Role");
            if (role != (int)AccountRole.Administrator)
            {
                return RedirectToPage("../Home");
            }
            var find = await _levelService.GetModelAsync(path: path, expression: c => c.IsDeleted == false, token: token);
            if (find == null)
            {
                return NotFound("Level Id Not Found!");
            }
            find.IsDeleted = true;
            if (id == null)
            {
                return NotFound();
            }
            if (id != Level.Id)
            {
                return BadRequest();
            }
            var post = await _postService.GetListAsync(path: StoredURI.Post, expression: c => c.IsDeleted == false && c.LevelId == id, token: token);
            if (post.Count > 0)
            {
                ViewData["message"] = "Some post have this level";
                Level = find;
                return Page();
            }
            await _levelService.Update(find, path: StoredURI.Level + "/" + Level.Id.ToString(), token: token);
            return RedirectToPage("./Index");
        }
    }
}
