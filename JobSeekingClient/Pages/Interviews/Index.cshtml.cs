using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
namespace JobSeekingClient.Pages.Interviews
{
    public class IndexModel : PageModel
    {
        private readonly 

        public IndexModel(AppCore.Context context)
        {
            _context = context;
        }

        public IList<Interview> Interview { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Interviews != null)
            {
                Interview = await _context.Interviews
                .Include(i => i.Application)
                .Include(i => i.Interviewer)
                .Include(i => i.Slot).ToListAsync();
            }
        }
    }
}
