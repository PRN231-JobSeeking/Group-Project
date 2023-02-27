using AppCore;
using AppCore.Models;
using AppRepository.Generic;
using AppRepository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRepository.Repositories.Implement
{
    internal class PostRepository : GenericRepository<Post>, IPostRepository
    {
        public PostRepository(Context context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }

        public Task<Post> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Post>> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
