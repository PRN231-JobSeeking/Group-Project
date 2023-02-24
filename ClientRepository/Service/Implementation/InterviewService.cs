using ClientRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientRepository.Service.Implementation
{
    public class InterviewService : BaseService<InterviewModel>, IInterviewService
    {
        public InterviewService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
        }
    }
}
