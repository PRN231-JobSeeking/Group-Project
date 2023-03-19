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
using ClientRepository.Service.Implementation;

namespace JobSeekingClient.Pages.Locations
{
    public class DeleteModel : PageModel
    {
        private readonly ILocationService _locationService;
        private readonly IPostService _postService;

        public DeleteModel(ILocationService locationService, IPostService postService)
        {
            _locationService = locationService;
            _postService=postService;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            string path = StoredURI.Location + "/" + id;
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
            var find = await _locationService.GetModelAsync(path: path, expression: c => c.IsDeleted == false, token: token);
            if (find == null)
            {
                return NotFound("Level Id Not Found!");
            }
            find.IsDeleted = true;
            if (id == null)
            {
                return NotFound();
            }
            if (id != Location.Id)
            {
                return BadRequest();
            }
            var post = await _postService.GetListAsync(path: StoredURI.Post + "/GetAll", expression: c => c.IsDeleted == false && c.LocationId == id, token: token);
            if (post.Count > 0)
            {
                ViewData["message"] = "Some post have this location";
                Location = find;
                return Page();
            }
            await _locationService.Delete(find, path: StoredURI.Location + "/" + Location.Id.ToString(), token: token);
            return RedirectToPage("./Index");
        }
    }
}
