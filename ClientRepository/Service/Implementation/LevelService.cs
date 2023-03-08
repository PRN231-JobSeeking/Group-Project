using ClientRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientRepository.Service.Implementation
{
    public class LevelService : BaseService<LevelModel>, ILevelService
    {
        public LevelService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
        }
    }
}
