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
    public class ViewApplicationsModel : PageModel
    {
        private readonly AppCore.Context _context;

        public ViewApplicationsModel(AppCore.Context context)
        {
            _context = context;
        }

        public IList<Application> Application { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Applications != null)
            {
                Application = await _context.Applications
                .Include(a => a.Applicant)
                .Include(a => a.Post).ToListAsync();
            }
        }
    }
}
