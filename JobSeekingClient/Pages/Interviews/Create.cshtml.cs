using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using AppCore;
using AppCore.Models;

namespace JobSeekingClient.Pages.Interviews
{
    public class CreateModel : PageModel
    {
        private readonly AppCore.Context _context;

        public CreateModel(AppCore.Context context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["ApplicationId"] = new SelectList(_context.Applications, "Id", "CV");
        ViewData["InterviewerId"] = new SelectList(_context.Accounts, "Id", "Address");
        ViewData["SlotId"] = new SelectList(_context.Set<Slot>(), "Id", "Id");
            return Page();
        }

        [BindProperty]
        public Interview Interview { get; set; }
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Interviews.Add(Interview);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
