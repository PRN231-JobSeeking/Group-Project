using ClientRepository.Models;
using ClientRepository.Service;
using ClientRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ClientRepository.Utils;

namespace JobSeekingClient.Pages.Accounts
{
    public class IndexAdminModel : PageModel
    {
        private readonly IAccountService _accountService;

        public IndexAdminModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public IList<AccountModel> Account { get; set; } = new List<AccountModel>();

        public async Task<IActionResult> OnGetAsync()
        {
            string? token = HttpContext.Session.GetString("token");
            int? test = HttpContext.Session.GetInt32("Role");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/Auth/Login");
            }
            if (test != (int)AccountRole.Administrator)
            {
                return RedirectToPage("/Home");
            }
            var list = await _accountService.GetListAsync(path: StoredURI.Account, expression: c => c.IsDeleted == false & c.RoleId!=1, param: null, token: token);
            if (list != null)
            {
                Account = list;
            }
            return Page();
        }
    }
}
