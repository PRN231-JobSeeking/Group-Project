using ClientRepository.Models;
using ClientRepository.Service;
using ClientRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JobSeekingClient.Pages.Accounts
{
    public class ProfileModel : PageModel
    {
        private readonly IAccountService _accountService;

        public ProfileModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public AccountModel Account { get; set; } = null!;

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
