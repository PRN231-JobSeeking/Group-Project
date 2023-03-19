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
using ClientRepository.Utils;
using ClientRepository;
using ClientRepository.Models;

namespace JobSeekingClient.Pages.Categories
{
    public class DeleteModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        private readonly IPostService _postService;

        public DeleteModel(ICategoryService categoryService, IPostService postService)
        {
            _categoryService = categoryService;
            _postService = postService;
        }

        [BindProperty]
      public CategoryModel Category { get; set; }

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
            string path = StoredURI.Category + "/" + id;
            var find = await _categoryService.GetModelAsync(path: path, expression: c => c.IsDeleted == false, token: token);
            if (find == null)
            {
                return NotFound();
            }
            Category = find;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            string path = StoredURI.Category + "/" + id;
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
            var post = await _postService.GetListAsync(path:StoredURI.Post+"/GetAll",expression: c=>c.IsDeleted==false && c.CategoryId==id,token:token);
            var find = await _categoryService.GetModelAsync(path: path, expression: c => c.IsDeleted == false, token: token);
            if(post.Count > 0) 
            {
                ViewData["message"] = "Some post have this category";
                Category = find;
                return Page();
            }
            if(find == null)
            {
                return NotFound("Category Id Not Found!");
            }
            find.IsDeleted = true;
            if (id == null)
            {
                return NotFound();
            }
            if (id != Category.Id)
            {
                return BadRequest();
            }
             await _categoryService.Delete(find, path: StoredURI.Category + "/" + Category.Id.ToString(), token: token);         
            return RedirectToPage("./Index");
        }
    }
}
