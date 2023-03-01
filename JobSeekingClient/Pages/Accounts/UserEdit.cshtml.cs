using ClientRepository.Models;
using ClientRepository.Service;
using ClientRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Policy;

namespace JobSeekingClient.Pages.Accounts
{
    public class UserEditModel : PageModel
    {
        private readonly IRoleService _roleService;
        private readonly IAccountService _accountService;

        public UserEditModel(IAccountService accountService, IRoleService roleService)
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
            if (test != 4)
            {
                return RedirectToPage("./HomePage");
            }
            string path = StoredURI.Account + "/" + id;
            var find = await _accountService.GetModelAsync(path: path, expression: c => c.IsDeleted == false, token: token);
            if (find == null)
            {
                return RedirectToPage("./HomePage");
            }
            Account = find;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            string? token = HttpContext.Session.GetString("token");
            string path = StoredURI.Account + "/" + Account.Id;
            var find = await _accountService.GetModelAsync(path: path, token: token);
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (!Account.Email.Equals(find.Email))
            {
                var list = await _accountService.GetListAsync(path: StoredURI.Account, expression: c => c.Email.Equals(Account.Email), param: null, token: token);
                if (list.Count() > 0)
                {
                    ViewData["messageemail"] = "Email already in use";
                    return Page();
                }
            }
            if (!Account.Phone.Equals(find.Phone))
            {
                var list = await _accountService.GetListAsync(path: StoredURI.Account, expression: c => c.Phone.Equals(Account.Phone), param: null, token: token);
                if (list.Count() > 0)
                {
                    ViewData["messagephone"] = "Phone number already in use";
                    return Page();
                }
            }

            await _accountService.Update(Account, path: StoredURI.Account + "/" + Account.Id.ToString(), token: token);
            return RedirectToPage("./Index");
        }
    }
}
