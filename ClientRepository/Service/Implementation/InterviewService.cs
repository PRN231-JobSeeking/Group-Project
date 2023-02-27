using ClientRepository.Extension;
using ClientRepository.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ClientRepository.Service.Implementation
{
    public class InterviewService : BaseService<InterviewModel>, IInterviewService
    {
        public InterviewService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
        }


        public async Task<IEnumerable<AccountModel>?> GetAvailableInterviewers(int slotId, DateOnly date, int applicationId, string? token)
        {
            var result = new List<AccountModel>();
            AddTokenHeader(token);
            Dictionary<string, string> param = new Dictionary<string, string>()
            {
                {"slotId", slotId.ToString() },
                {"date", date.ToString() }
            };
            var response = await Client.GetAsync(StoredURI.Interviews + "/create/available/" + applicationId.ToString()+ HttpRequestSupport.GetQueryPath(param));
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadFromJsonAsync<List<AccountModel>>();
            } else
            {
                throw new Exception("InvalidTime");
            }
            return result;
        }

        
    }
}
