using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ClientRepository.Service;
using ClientRepository.Service.Implementation;
using ClientRepository;
using ClientRepository.Models;

namespace JobSeekingClient.Pages.Accounts
{
    public class CreateModel : PageModel
    {
        private readonly IRoleService _roleService;
        private readonly IAccountService _accountService;

        public CreateModel(IAccountService accountService, IRoleService roleService)
        {
            _accountService = accountService;
            _roleService = roleService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["RoleId"] = new SelectList(await _roleService.GetListAsync(path: StoredURI.Role), "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Account Account { get; set; } = null!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid)
            {
                return Page();
            }
            await _accountService.Add(Account, path: StoredURI.Account, token: null);
            return RedirectToPage("./Index");
        }
    }
}
