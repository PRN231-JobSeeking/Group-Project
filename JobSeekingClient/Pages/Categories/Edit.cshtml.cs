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

namespace JobSeekingClient.Pages.Categories
{
    public class EditModel : PageModel
    {
        private readonly ICategoryService _categoryService;

        public EditModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
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
            var category = await _categoryService.GetModelAsync(expression: c => c.Name.ToLower().Equals(Category.Name.ToLower())
                                                               , path: StoredURI.Category + "/" + Category.Id.ToString(), token: token);
            if(category != null)
            {
                ViewData["Error"] = "Already exist category name!";
                await OnGetAsync(Category.Id);
                return Page();
            }
            await _categoryService.Update(Category, path: StoredURI.Category + "/" + Category.Id.ToString(), token: token);
            return RedirectToPage("./Index");
        }
    }
}
