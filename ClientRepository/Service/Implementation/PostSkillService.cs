using ClientRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientRepository.Service.Implementation
{
    public class PostSkillService : BaseService<PostSkillModel>, IPostSkillService
    {
        public PostSkillService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
        }
    }
}
