using ClientRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientRepository.Service.Implementation
{
    public class LocationService : BaseService<LocationModel>, ILocationService
    {
        public LocationService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
        }
    }
}
