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

namespace JobSeekingClient.Pages.Locations
{
    public class DetailsModel : PageModel
    {
        private readonly ILocationService _locationService;

        public DetailsModel(ILocationService locationService)
        {
            _locationService = locationService;
        }

      public LocationModel Location { get; set; }

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
            string path = StoredURI.Location + "/" + id;
            var find = await _locationService.GetModelAsync(path: path, token: token);
            if (find == null)
            {
                return NotFound();
            }
            Location = find;
            return Page();
        }
    }
}
