using ClientRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientRepository.Service.Implementation
{
    public class CategoryService : BaseService<CategoryModel>, ICategoryService
    {
        public CategoryService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
        }
    }
}
