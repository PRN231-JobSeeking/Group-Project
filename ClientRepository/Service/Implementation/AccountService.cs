using ClientRepository.Extension;
using ClientRepository.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ClientRepository.Service.Implementation
{
    public class AccountService : BaseService<AccountModel>, IAccountService
    {
        public AccountService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
        }
    }
}
