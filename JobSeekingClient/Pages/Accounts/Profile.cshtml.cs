using ClientRepository.Models;
using ClientRepository.Service;
using ClientRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ClientRepository.Utils;

namespace JobSeekingClient.Pages.Accounts
{
    public class ProfileModel : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly ISkillService _skillService;

        public ProfileModel(IAccountService accountService, ISkillService skillService)
        {
            _accountService = accountService;
            _skillService = skillService;
        }

        public AccountModel Account { get; set; } = null!;

        public IList<Skill> Skills { get; set; } = new List<Skill>();

        public async Task<IActionResult> OnGetAsync(int? id)
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
            string path = StoredURI.Account + "/" + id;
            var find = await _accountService.GetModelAsync(path: path, expression: c => c.IsDeleted == false, token: token);
          
            

            if (find == null)
            {
                return RedirectToPage("/Home");
            }
            Account = find;
           
            foreach (var skill in Account.UserSkill) 
            {

                var skilllist = await _skillService.GetListAsync(path: StoredURI.Skill, expression: c => c.IsDeleted == false && c.Id==skill.SkillId, token: token);
                Skills.Add(skilllist.FirstOrDefault());
            }
            return Page();
        }
    }
}
