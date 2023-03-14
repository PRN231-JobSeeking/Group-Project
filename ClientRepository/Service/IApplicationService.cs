using ClientRepository.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientRepository.Service
{
    public interface IApplicationService : IBaseService<ApplicationModel>
    {
        Task<bool> Create(int postId, IFormFile file, string? token);

    }
}
