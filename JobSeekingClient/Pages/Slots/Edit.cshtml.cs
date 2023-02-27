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
using ClientRepository;
using ClientRepository.Models;
using ClientRepository.Service.Implementation;

namespace JobSeekingClient.Pages.Slots
{
    public class EditModel : PageModel
    {
        private readonly ISlotService _slotService;

        public EditModel(ISlotService slotService)
        {
            _slotService = slotService;
        }

        [BindProperty]
        public SlotModel Slot { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            string path = StoredURI.Slot + "/" + id;
            var find = await _slotService.GetModelAsync(path: path);
            if (find == null)
            {
                return NotFound();
            }
            Slot = find;
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
            var token = HttpContext.Session.GetString("token");
            await _slotService.Update(Slot, path: StoredURI.Slot + "/" + Slot.Id.ToString(), token: null);
            return RedirectToPage("./Index");
        }
    }
}
