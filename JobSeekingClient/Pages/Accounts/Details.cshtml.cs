﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ClientRepository.Service;
using ClientRepository.Service.Implementation;
using ClientRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using ClientRepository.Models;
using Microsoft.AspNetCore.Authorization;

namespace JobSeekingClient.Pages.Accounts
{
   
    public class DetailsModel : PageModel
    {
        
        private readonly IAccountService _accountService;

        public DetailsModel(IAccountService accountService)
        {
            _accountService= accountService;
        }

        public AccountModel Account { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            string path = StoredURI.Account + "/" + id;
            var token = HttpContext.Session.GetString("token");
            var find = await _accountService.GetModelAsync(path: path,token: token);
            if (find == null)
            {
                return NotFound();
            }
            Account = find;
            return Page();
        }
    }
}
