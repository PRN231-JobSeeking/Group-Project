using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ClientRepository.Service;
using ClientRepository.Models;
using ClientRepository.Service.Implementation;
using ClientRepository;
using System.Diagnostics;

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
                Debug.WriteLine("Login.OnPost: list count: " + list.Count);
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
                    return RedirectToPage("/Home");
                }
                if (user.RoleId == 3)
                {
                    return RedirectToPage("/Interviews/HomePageInterviewer");
                }
                if (user.RoleId == 4)
                {
                    return RedirectToPage("/Home");
                }
            }
            ViewData["message"] = "Wrong email and password! Please try again!";
            return Page();
        }
    }
}

