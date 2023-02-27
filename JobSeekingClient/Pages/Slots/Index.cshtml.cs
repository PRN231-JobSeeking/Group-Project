using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ClientRepository.Service;
using ClientRepository.Models;
using ClientRepository;

namespace JobSeekingClient.Pages.Slots
{
    public class IndexModel : PageModel
    {
        private readonly ISlotService _slotService;

        public IndexModel(ISlotService slotService)
        {
            _slotService= slotService;
        }

        public IList<SlotModel> Slot { get;set; } = default!;

        public async Task OnGetAsync()
        {
            var token = HttpContext.Session.GetString("token");
            var slots = await _slotService.GetListAsync(path: StoredURI.Slot, token: token);
            if (slots != null)
            {
                Slot = slots;
            }
        }
    }
}
