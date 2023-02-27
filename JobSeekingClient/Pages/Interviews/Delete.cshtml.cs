using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AppCore;
using AppCore.Models;

namespace JobSeekingClient.Pages.Interviews
{
    public class DeleteModel : PageModel
    {
        private readonly AppCore.Context _context;

        public DeleteModel(AppCore.Context context)
        {
            _context = context;
        }

        [BindProperty]
      public Interview Interview { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Interviews == null)
            {
                return NotFound();
            }

            var interview = await _context.Interviews.FirstOrDefaultAsync(m => m.ApplicationId == id);

            if (interview == null)
            {
                return NotFound();
            }
            else 
            {
                Interview = interview;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Interviews == null)
            {
                return NotFound();
            }
            var interview = await _context.Interviews.FindAsync(id);

            if (interview != null)
            {
                Interview = interview;
                _context.Interviews.Remove(Interview);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
