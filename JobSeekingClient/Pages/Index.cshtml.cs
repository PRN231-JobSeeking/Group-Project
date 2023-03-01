using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JobSeekingClient.Pages
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetInt32("RoleId");
            if (role == null)
            {
                return RedirectToAction("Get", "Login");
            }

            return Page();
        }
    }
}
