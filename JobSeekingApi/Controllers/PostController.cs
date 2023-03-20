using AppCore.Models;
using AppRepository.UnitOfWork;
using ClientRepository.Models;
using Microsoft.AspNetCore.Authorization;
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
            if (list == null)
            {
                return BadRequest();
            }
            if (list.Count() == 0)
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
            if (post == null)
            {
                return BadRequest();
            }
            return Ok(post);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Post>> Post(PostDTO postModel)
        {
            var task = unitOfWork.PostRepository.Add(new Post()
            {
                Title = postModel.Title,
                Amount = postModel.Amount,
                CategoryId = postModel.CategoryId,
                CreateDate = postModel.CreateDate,
                Description = postModel.Description,
                EndDate = postModel.EndDate,
                IsDeleted = false,
                LevelId = postModel.LevelId,
                LocationId = postModel.LocationId,
                StartDate = postModel.StartDate,
                Status = postModel.Status,
            });
            await task;
            if (task.IsCompletedSuccessfully)
            {
                return Ok();
            }
            return BadRequest();
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Putpost(int id, PostDTO post)
        {
            if (id != post.Id)
            {
                return BadRequest();
            }
            var postInDb = await unitOfWork.PostRepository.GetFirst(a => a.Id == post.Id);

            if (postInDb == null)
            {
                return BadRequest();
            }
            postInDb.Status = post.Status;
            postInDb.StartDate = post.StartDate;
            postInDb.Amount = post.Amount;
            postInDb.Title = post.Title;
            postInDb.CategoryId = post.CategoryId;
            postInDb.CreateDate = post.CreateDate;
            postInDb.Description = post.Description;
            postInDb.EndDate = post.EndDate;
            postInDb.IsDeleted = post.IsDeleted;
            postInDb.LevelId = post.LevelId;
            postInDb.LocationId = post.LocationId;

            await unitOfWork.PostRepository.Update(postInDb);
            return StatusCode(StatusCodes.Status200OK);
        }
    }
}
