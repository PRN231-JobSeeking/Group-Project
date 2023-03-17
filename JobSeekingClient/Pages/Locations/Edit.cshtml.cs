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

namespace JobSeekingClient.Pages.Locations
{
    public class EditModel : PageModel
    {
        private readonly ILocationService _locationService;

        public EditModel(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [BindProperty]
        public LocationModel Location { get; set; } = default!;

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
            string path = StoredURI.Location + "/" + id;
            var find = await _locationService.GetModelAsync(path: path, expression: c => c.IsDeleted == false, token: token);
            if (find == null)
            {
                return NotFound();
            }
            Location = find;
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
            var location = await _locationService.GetModelAsync(expression: c => c.Name.ToLower().Equals(Location.Name.ToLower())
                                                               , path: StoredURI.Location + "/" + Location.Id.ToString(), token: token);
            if (location != null)
            {
                ViewData["Error"] = "Already exist location name!";
                await OnGetAsync(Location.Id);
                return Page();
            }
            await _locationService.Update(Location, path: StoredURI.Location + "/" + Location.Id.ToString(), token: token);
            return RedirectToPage("./Index");
        }
    }
}
