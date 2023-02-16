using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ClientRepository.Service;
using ClientRepository.Models;

namespace JobSeekingClient.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly IAuthenService _authenService;
        public LoginModel(IAuthenService authenService)
        {
            _authenService = authenService;
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
           var token= await _authenService.Login(Credential);
            HttpContext.Session.SetString("token", token.ToString());
            if (!string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/Accounts/Index");
            }
            return Page();
        }

    }  
}

