using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ClientRepository.Service;
using ClientRepository.Models;

namespace JobSeekingClient.Pages.Post
{
    public class PostDeleteModel : PageModel
    {
        private readonly IPostModelService _postService;

        public PostDeleteModel(IPostModelService postService)
        {
            _postService= postService;
        }

        [BindProperty]
        public PostModel Post { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var token = HttpContext.Session.GetString("token");
            var post = await _postService.GetModelAsync(path: "hr-delete-post/" + id, token: token);

            if (post == null)
            {
                return NotFound();
            }
            else 
            {
                Post = post;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var token = HttpContext.Session.GetString("token");
            try
            {
                var check = await _postService.Delete(model: Post, path: "hr-delete-post/" + id, token: token);
            } catch(Exception ex)
            {
                ViewData["Message"] = ex.Message;
                return await OnGetAsync(id);
            }
            
            return RedirectToPage("/Home");
        }
    }
}
