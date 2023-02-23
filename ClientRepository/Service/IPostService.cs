using ClientRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientRepository.Service
{
    public interface IPostService : IBaseService<PostDTO>
    {
        Task<PostDTO> GetModelAsync(int id);
    }
}
