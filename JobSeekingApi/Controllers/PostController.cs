using AppCore.Models;
using AppRepository.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace JobSeekingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public PostController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<Post>>> GetAll()
        {
            var list = unitOfWork.PostRepository.GetAll().Result;
            if(list == null)
            {
                return BadRequest();
            }
            if(list.Count() == 0)
            {
                return NotFound();
            }
            return Ok(list);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Post>> Get(int id)
        {
            var post = unitOfWork.PostRepository.Get(id).Result;
            if(post == null)
            {
                return BadRequest();
            }
            return Ok(post);
        }

    }
}
