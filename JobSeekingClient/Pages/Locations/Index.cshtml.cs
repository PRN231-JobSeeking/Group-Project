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
    public class IndexModel : PageModel
    {
        private readonly ILocationService _locationService;

        public IndexModel(ILocationService locationService)
        {
            _locationService = locationService;
        }

        public IList<LocationModel> Locations { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync()
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
            var list = await _locationService.GetListAsync(path: StoredURI.Location, expression: null, param: null, token: token);
            if (list != null)
            {
                Locations = list;
            }
            return Page();
        }
    }
}
