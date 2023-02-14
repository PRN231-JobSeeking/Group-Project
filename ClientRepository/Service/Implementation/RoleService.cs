using ClientRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientRepository.Service.Implementation
{
    public class RoleService : BaseService<Role>, IRoleService
    {
        public RoleService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
        }
    }
}
