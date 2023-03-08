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
using ClientRepository.Utils;
using ClientRepository;

namespace JobSeekingClient.Pages.Levels
{
    public class DetailsModel : PageModel
    {
        private readonly ILevelService _levelService;

        public DetailsModel(ILevelService levelService)
        {
            _levelService = levelService;
        }

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
                return RedirectToPage("../HomePage");
            }
            string path = StoredURI.Level + "/" + id;
            var find = await _levelService.GetModelAsync(path: path, token: token);
            if (find == null)
            {
                return NotFound();
            }
            Level = find;
            return Page();
        }
    }
}
