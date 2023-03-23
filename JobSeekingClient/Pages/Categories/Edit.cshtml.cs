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
using ClientRepository.Utils;
using ClientRepository;
using ClientRepository.Models;
using ClientRepository.Service.Implementation;

namespace JobSeekingClient.Pages.Categories
{
    public class EditModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        private readonly IPostService _postService;

        public EditModel(ICategoryService categoryService, IPostService postService)
        {
            _categoryService = categoryService;
            _postService = postService;
        }

        [BindProperty]
        public CategoryModel Category { get; set; } = default!;

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
            var categories = await _categoryService.GetListAsync(expression: c => c.Name.ToLower().Equals(Category.Name.ToLower()), path: StoredURI.Category, token: token);
            if (categories != null && categories.Count > 0)
            {
                foreach(var category in categories)
                {
                    if(category.Id != Category.Id && category.IsDeleted == false)
                    {
                        ViewData["Error"] = "Already exist category name!";
                        await OnGetAsync(Category.Id);
                        return Page();
                    }
                }
            }
            var post = await _postService.GetListAsync(path: StoredURI.Post + "/GetAll", expression: c => c.IsDeleted == false && c.CategoryId == Category.Id, token: token);
            if (post.Count > 0)
            {
                ViewData["Error"] = "Some post have this category";
                await OnGetAsync(Category.Id);
                return Page();
            }
            await _categoryService.Update(Category, path: StoredURI.Category + "/" + Category.Id.ToString(), token: token);
            return RedirectToPage("./Index");
        }
    }
}
