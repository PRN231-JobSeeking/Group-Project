using ClientRepository.Models;
using ClientRepository.Service;
using ClientRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Common;
using ClientRepository.Utils;

namespace JobSeekingClient.Pages.Accounts
{
    public class ChangePassModel : PageModel
    {
        private readonly IRoleService _roleService;
        private readonly IAccountService _accountService;

        public ChangePassModel(IAccountService accountService, IRoleService roleService)
        {
            _accountService = accountService;
            _roleService = roleService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["RoleId"] = new SelectList(await _roleService.GetListAsync(path: StoredURI.Role, token: HttpContext.Session.GetString("token")), "Id", "Name");
            return Page();
        }

        [BindProperty]
        public ChangePass Account { get; set; } = null!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {

            string? token = HttpContext.Session.GetString("token");
            int? test = HttpContext.Session.GetInt32("Role");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/Auth/Login");
            }
            if (test == (int)AccountRole.Administrator || test == (int)AccountRole.HR)
            {
                return RedirectToPage("/Home");
            }
            string path = StoredURI.Account + "/" + HttpContext.Session.GetInt32("UserId");
            var find = await _accountService.GetModelAsync(path: path, expression: c => c.IsDeleted == false, token: token);
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if(find == null)
            {
                return RedirectToPage("/Home");
            }
            if(!find.Password.Equals(Account.Password))
            {
                ViewData["message"] = "Incorrect password!";
                return Page();
            }
            find.Password= Account.NewPassword;
            await _accountService.Update(find, path: path, token: token);
            return RedirectToPage("/Home");
        }
    }
}
