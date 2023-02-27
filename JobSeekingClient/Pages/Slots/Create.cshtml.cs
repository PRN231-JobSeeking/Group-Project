using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using AppCore;
using AppCore.Models;
using ClientRepository.Models;
using ClientRepository.Service;
using ClientRepository;

namespace JobSeekingClient.Pages.Slots
{
    public class CreateModel : PageModel
    {
        private readonly ISlotService _slotService;

        public CreateModel(ISlotService slotService)
        {
            _slotService= slotService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public SlotModel Slot { get; set; } = null!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid)
            {
                return Page();
            }

            var token = HttpContext.Session.GetString("token");            
            await _slotService.Add(Slot, path: StoredURI.Slot,token: token);

            return RedirectToPage("./Index");
        }
    }
}
