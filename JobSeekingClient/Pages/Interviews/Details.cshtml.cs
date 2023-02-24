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
    public class DetailsModel : PageModel
    {
        private readonly AppCore.Context _context;

        public DetailsModel(AppCore.Context context)
        {
            _context = context;
        }

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
    }
}
