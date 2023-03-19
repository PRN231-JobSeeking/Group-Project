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
using ClientRepository.Utils;
using ClientRepository.Models;
using ClientRepository;

namespace JobSeekingClient.Pages.Levels
{
    public class CreateModel : PageModel
    {
        private readonly ILevelService _levelService;

        public CreateModel(ILevelService levelService)
        {
            _levelService = levelService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public LevelModel Level { get; set; }
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
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

            var level = await _levelService.GetModelAsync(expression: c => c.Name.ToLower().Equals(Level.Name.ToLower())
                                                               , path: StoredURI.Level + "/" + Level.Id.ToString(), token: token);
            if (level != null)
            {
                ViewData["Error"] = "Already exist level name!";
                return Page();
            }

            await _levelService.Add(Level, path: StoredURI.Level, token: token);
            return RedirectToPage("./Index");
        }
    }
}
