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
    public class DeleteModel : PageModel
    {
        private readonly IAccountService _accountService;

        public DeleteModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            if(id != Account.Id)
            {
                return BadRequest();
            }
            await _accountService.Delete(Account, path: StoredURI.Account + "/" + Account.Id.ToString(), token: null);
            return RedirectToPage("./Index");
        }
    }
}
