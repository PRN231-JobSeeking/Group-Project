using ClientRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientRepository.Service.Implementation
{
    public class SkillService : BaseService<Skill>, ISkillService
    {
        public SkillService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
        }
    }
}
