using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClientRepository.Service;
using ClientRepository.Models;
using ClientRepository;

namespace JobSeekingClient.Pages.Accounts
{
    public class EditModel : PageModel
    {
        private readonly IRoleService _roleService;
        private readonly IAccountService _accountService;

        public EditModel(IAccountService accountService, IRoleService roleService)
        {
            _accountService = accountService;
            _roleService = roleService;
        }
        
        [BindProperty]
        public AccountModel Account { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            string? token = HttpContext.Session.GetString("token");
            int? test = HttpContext.Session.GetInt32("Role");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/Auth/Login");
            }
            if (test != 1)
            {
                return RedirectToPage("./HomePage");
            }
            string path = StoredURI.Account + "/" + id;
            var find = await _accountService.GetModelAsync(path: path, expression: c => c.IsDeleted == false, token: token);
            if (find == null)
            {
                return NotFound();
            }
            Account = find;
            ViewData["RoleId"] = new SelectList(await _roleService.GetListAsync(path: StoredURI.Role, token: token,expression:c=>!c.Name.Equals("Administrator")), "Id", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            string? token = HttpContext.Session.GetString("token");
            if (!ModelState.IsValid)
            {
                ViewData["RoleId"] = new SelectList(await _roleService.GetListAsync(path: StoredURI.Role, token: token), "Id", "Name");
                return Page();
            }

            await _accountService.Update(Account, path: StoredURI.Account + "/" + Account.Id.ToString(), token: token);
            return RedirectToPage("./IndexAdmin");
        }
    }
}
