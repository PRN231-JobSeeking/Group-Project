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

namespace JobSeekingClient.Pages.Interviews
{
    public class EditModel : PageModel
    {
        private readonly AppCore.Context _context;

        public EditModel(AppCore.Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Interview Interview { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Interviews == null)
            {
                return NotFound();
            }

            var interview =  await _context.Interviews.FirstOrDefaultAsync(m => m.ApplicationId == id);
            if (interview == null)
            {
                return NotFound();
            }
            Interview = interview;
           ViewData["ApplicationId"] = new SelectList(_context.Applications, "Id", "CV");
           ViewData["InterviewerId"] = new SelectList(_context.Accounts, "Id", "Address");
           ViewData["SlotId"] = new SelectList(_context.Set<Slot>(), "Id", "Id");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Interview).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InterviewExists(Interview.ApplicationId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool InterviewExists(int id)
        {
          return _context.Interviews.Any(e => e.ApplicationId == id);
        }
    }
}
