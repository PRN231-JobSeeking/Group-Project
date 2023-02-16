using ClientRepository.Models;
using ClientRepository;
using ClientRepository.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using AppCore.Models;

namespace JobSeekingClient.Pages.Auth
{
    public class RegisterModel : PageModel
    {

        private readonly IAccountService _accountService;
        private readonly ISkillService _skillService;
        private readonly IUserSkillService _userskillService;

        public RegisterModel(IAccountService accountService, ISkillService skillService, IUserSkillService userskillService)
        {
            _accountService = accountService;
            _skillService = skillService;
            _userskillService = userskillService;
        }


        public async Task<IActionResult> OnGetAsync()
        {
           
            this.Skills = this.PopulateSkills();           
            return Page();
        }

        private List<SelectListItem> PopulateSkills()
        {
            List<ClientRepository.Models.Skill> skills = _skillService.GetListAsync(path: StoredURI.Skill, expression: null, param: null, token: null).Result;
            List<SelectListItem> SkillList = (from p in skills
                                               select new SelectListItem
                                               {
                                                   Text = p.Name,
                                                   Value = p.Id.ToString()
                                               }).ToList();

            return SkillList;
        }

        [BindProperty]
        public RegisterDTO RegisterUser { get; set; }

        public  List<SelectListItem> Skills { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(string[] SkillIds)
        {
        
            if (!ModelState.IsValid)
            {
                return Page();
            }         
            AccountModel user = new AccountModel()
            {                          
                Email = RegisterUser.Email,
                FirstName = RegisterUser.FirstName,
                LastName = RegisterUser.LastName,
                Password = RegisterUser.Password,            
                Phone = RegisterUser.Phone,
                Address = RegisterUser.Address,
                IsLockout=false,
                RoleId=3    
            };
            await _accountService.Add(user, path: StoredURI.Account, token: null);
            IList<AccountModel> list = await _accountService.GetListAsync(path: StoredURI.Account, expression: null, param: null, token: null);
            AccountModel tmp = list.FirstOrDefault(e => e.Email == user.Email);       
            if (tmp != null)
            {                
                foreach (string Id in SkillIds)
                {                   
                    ClientRepository.Models.UserSkill uktmp = new ClientRepository.Models.UserSkill();
                    uktmp.AccountId = tmp.Id;
                    uktmp.SkillId = int.Parse(Id);
                    await _userskillService.Add(uktmp, path: StoredURI.UserSkill, token: null);
                }
            }
            return RedirectToPage("Login");
        }
    }
    public class RegisterDTO
    {
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = null!;
        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
        [Required]
        public string Address { get; set; } = null!;
        [Required]
        public string Phone { get; set; } = null!;
    }

   

}

