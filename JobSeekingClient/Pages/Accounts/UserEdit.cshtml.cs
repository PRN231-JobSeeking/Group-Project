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
        private readonly IUserSkillService _userskillService;
        private readonly ISkillService _skillService;

        public UserEditModel(IAccountService accountService, IRoleService roleService, IUserSkillService userskillService, ISkillService skillService)
        {
            _accountService = accountService;
            _roleService = roleService;
            _userskillService = userskillService;
           _skillService = skillService;
        }
        public async Task<List<Skill>> listskillAsync()
        {
            List<Skill> skill = await _skillService.GetListAsync(path: StoredURI.Skill, token: HttpContext.Session.GetString("token"));
            List<UserSkill> userSkills= await _userskillService.GetListAsync(path: StoredURI.UserSkill, expression: c=>c.AccountId==Account.Id,token: HttpContext.Session.GetString("token"));
            foreach(var item in userSkills)
            {
                   var skillSkill = skill.FirstOrDefault(c => c.Id == item.SkillId);
                    skill.Remove(skillSkill);              
            }
            if(skill.Count()==0)
            {
                return null;
            }
            return skill;
        }
        [BindProperty]
        public AccountModel Account { get; set; } = default!;
        [BindProperty]
        public string skillid { get; set; }

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
            if(listskillAsync().Result!=null)
            {
                ViewData["SkillId"] = new SelectList(await listskillAsync(), "Id", "Name");
            }
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
                if (listskillAsync().Result != null)
                {
                    ViewData["SkillId"] = new SelectList(await listskillAsync(), "Id", "Name");
                }
                return Page();
            }
            if (!Account.Email.Equals(find.Email))
            {
                var list = await _accountService.GetListAsync(path: StoredURI.Account, expression: c => c.Email.Equals(Account.Email), param: null, token: token);
                if (list.Count() > 0)
                {
                    if (listskillAsync().Result != null)
                    {
                        ViewData["SkillId"] = new SelectList(await listskillAsync(), "Id", "Name");
                    }
                    ViewData["messageemail"] = "Email already in use";
                    return Page();
                }
            }
            if (!Account.Phone.Equals(find.Phone))
            {
                var list = await _accountService.GetListAsync(path: StoredURI.Account, expression: c => c.Phone.Equals(Account.Phone), param: null, token: token);
                if (list.Count() > 0)
                {
                    if (listskillAsync().Result != null)
                    {
                        ViewData["SkillId"] = new SelectList(await listskillAsync(), "Id", "Name");
                    }
                    ViewData["messagephone"] = "Phone number already in use";
                    return Page();
                }
            }
            if(!string.IsNullOrEmpty(skillid))
            {
                ClientRepository.Models.UserSkill uktmp = new ClientRepository.Models.UserSkill();
                uktmp.AccountId = Account.Id;
                uktmp.SkillId = int.Parse(skillid);
                await _userskillService.Add(uktmp, path: StoredURI.UserSkill, token: null);
            }
            await _accountService.Update(Account, path: StoredURI.Account + "/" + Account.Id.ToString(), token: token);
            return RedirectToPage("/Home");
        }
    }
}
