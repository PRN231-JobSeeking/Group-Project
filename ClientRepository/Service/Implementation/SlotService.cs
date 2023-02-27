using ClientRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientRepository.Service.Implementation
{
    public class SlotService : BaseService<SlotModel>, ISlotService
    {
        public SlotService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
        }
    }
}
