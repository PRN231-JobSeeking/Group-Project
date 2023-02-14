using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ClientRepository.Service;
using ClientRepository.Service.Implementation;
using ClientRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using ClientRepository.Models;

namespace JobSeekingClient.Pages.Accounts
{
    public class DetailsModel : PageModel
    {
        private readonly IAccountService _accountService;

        public DetailsModel(IAccountService accountService)
        {
            _accountService= accountService;
        }

        public Account Account { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            string path = StoredURI.Account + "/" + id;
            var find = await _accountService.GetModelAsync(path: path);
            if (find == null)
            {
                return NotFound();
            }
            Account = find;
            return Page();
        }
    }
}
