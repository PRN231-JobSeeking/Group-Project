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
using ClientRepository.Models;
using ClientRepository;
using ClientRepository.Utils;

namespace JobSeekingClient.Pages.Locations
{
    public class CreateModel : PageModel
    {
        private readonly ILocationService _locationService;

        public CreateModel(ILocationService locationService)
        {
            _locationService = locationService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public LocationModel Location { get; set; }
        

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
            await _locationService.Add(Location, path: StoredURI.Location, token: token);
            return RedirectToPage("./Index");
        }
    }
}
