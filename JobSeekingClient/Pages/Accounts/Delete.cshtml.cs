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
using NuGet.Common;
using System.IO;

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
        public AccountModel Account { get; set; } = null!;

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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            string path = StoredURI.Account + "/" + id;
            string? token = HttpContext.Session.GetString("token");
            var find = await _accountService.GetModelAsync(path: path, expression: c => c.IsDeleted == false, token: token);
            find.IsDeleted = true;
            if (id == null)
            {
                return NotFound();
            }
            if (id != Account.Id)
            {
                return BadRequest();
            }
            await _accountService.Update(find, path: StoredURI.Account + "/" + Account.Id.ToString(), token: token);
            return RedirectToPage("./IndexAdmin");
        }
    }
}
