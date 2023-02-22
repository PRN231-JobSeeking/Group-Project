using AppCore.Models;
using AppRepository.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace JobSeekingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public ApplicationController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<bool>> Create([FromForm]int postId, IFormFile file)
        {
            if (postId <= 0 || file == null)
            {
                return BadRequest();
            }
            string ext = Path.GetExtension(file.FileName);
            if(!ext.Equals(".pdf"))
            {
                return NotFound("File must have extension .pdf");
            }
            string id = Guid.NewGuid().ToString();
            if(!Directory.Exists(Directory.GetCurrentDirectory() + "/images"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/images");
            }
            string filePath = Directory.GetCurrentDirectory() + $"/images/{id}{ext}";
            using (var stream = System.IO.File.Create(filePath))
            {
                file.CopyTo(stream);
            }
            var aplication = new Application()
            {
                PostId = postId,
                CV = filePath,
                ApplicantId = 2,
                Status = true,
            };
            await unitOfWork.ApplicationRepository.Add(aplication);
            return Ok(true);
        }
    }
}
