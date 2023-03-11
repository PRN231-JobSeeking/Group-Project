using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ClientRepository.Service;
using ClientRepository.Models;
using ClientRepository.Service.Implementation;
using ClientRepository;

namespace JobSeekingClient.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly IAuthenService _authenService;
        private readonly IAccountService _accountService;
        public LoginModel(IAuthenService authenService, IAccountService accountService)
        {
            _authenService = authenService;
            _accountService = accountService;
        }

        [BindProperty]
        public UserLogin Credential { get; set; }


        public IActionResult OnGet()
        {

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }
            var token = await _authenService.Login(Credential);
            if (!string.IsNullOrEmpty(token))
            {
                token = token.Replace("\"", "");
                var list = await _accountService.GetListAsync(path: StoredURI.Account, expression: c => c.IsDeleted == false, param: null, token: token);
                var user = list.FirstOrDefault(a => a.Email == Credential.Email);
                HttpContext.Session.SetString("token", token.ToString());
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetInt32("Role", user.RoleId);
                if (user.RoleId == 1)
                {
                    return RedirectToPage("/Accounts/IndexAdmin");
                }
                if (user.RoleId == 2)
                {
                    return RedirectToPage("/Accounts/Index");
                }
                if (user.RoleId == 3)
                {
                    return RedirectToPage("/Home/Home");
                }
                if (user.RoleId == 4)
                {
                    return RedirectToPage("/Accounts/Index");
                }
            }
            ViewData["message"] = "Wrong email and password! Please try again!";
            return Page();
        }
    }
}

