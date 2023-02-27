using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AppCore;
using AppCore.Models;
using ClientRepository.Models;
using ClientRepository.Service;
using ClientRepository;

namespace JobSeekingClient.Pages.Slots
{
    public class DetailsModel : PageModel
    {
        private readonly ISlotService _slotService;

        public DetailsModel(ISlotService slotService)
        {
            _slotService= slotService;
        }

        public SlotModel Slot { get; set; } = null!;

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
    }
}
