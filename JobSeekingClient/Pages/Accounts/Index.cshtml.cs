using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ClientRepository.Service;
using ClientRepository;
using ClientRepository.Models;

namespace JobSeekingClient.Pages.Accounts
{
    public class IndexModel : PageModel
    {
        private readonly IAccountService _accountService;

        public IndexModel(IAccountService accountService)
        {
            _accountService= accountService;
        }

        public IList<Account> Account { get;set; } = new List<Account>();

        public async Task OnGetAsync()
        {
            var list = await _accountService.GetListAsync(path: StoredURI.Account, expression: null, param: null, token: null);
            if(list != null)
            {
                Account = list;
            }
        }
    }
}
