using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ClientRepository.Service;
using ClientRepository;
using ClientRepository.Models;
using AppCore.Models;
using ClientRepository.Service.Implementation;

namespace JobSeekingClient.Pages.Slots
{
    public class DeleteModel : PageModel
    {
        private readonly ISlotService _slotService;

        public DeleteModel(ISlotService slotService)
        {
            _slotService = slotService;
        }

        [BindProperty]
      public SlotModel Slot { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }
            var token = HttpContext.Session.GetString("token");
            string path = StoredURI.Slot + "/" + id;
            var slot = await _slotService.GetModelAsync(path: path, token: token);

            if (slot == null)
            {
                return NotFound();
            }
            else 
            {
                Slot = slot;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (id != Slot.Id)
            {
                return BadRequest();
            }
            var token = HttpContext.Session.GetString("token");
            await _slotService.Delete(Slot, path: StoredURI.Slot + "/" + Slot.Id.ToString(), token: token);
            return RedirectToPage("./Index");
        }
    }
}
