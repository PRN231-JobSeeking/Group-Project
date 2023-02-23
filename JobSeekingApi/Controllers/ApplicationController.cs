using AppCore.Models;
using AppRepository.UnitOfWork;
using JobSeekingClient.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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

        [HttpGet]
        [Route("Get/Id/{aplicationId}")]
        public async Task<IActionResult> GetApplication([FromRoute] int aplicationId)
        {
            var aplication = unitOfWork.ApplicationRepository.Get(a => a.Id == aplicationId).Result.FirstOrDefault();
            if(aplication == null)
            {
                return NotFound("Not found aplicationId!");
            }
            return Ok(aplication);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<bool>> Create([FromForm]int postId, IFormFile file)
        {
            Trace.WriteLine("Test");
            if (postId <= 0 || file == null)
            {
                return BadRequest();
            }
            var postIdInDb = unitOfWork.PostRepository.Get(p => p.Id == postId).Result.FirstOrDefault();
            if(postIdInDb == null)
            {
                return NotFound("Not found post id!");
            }
            string ext = Path.GetExtension(file.FileName);
            if(!ext.Equals(".pdf"))
            {
                return NotFound("File must have extension .pdf");
            }
            string id = Guid.NewGuid().ToString();
            if(!Directory.Exists(Directory.GetCurrentDirectory() + "/wwwroot")) {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/wwwroot");
            }
            if (!Directory.Exists(Directory.GetCurrentDirectory() + "/wwwroot/images"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/wwwroot/images");
            }
            string filePath = Directory.GetCurrentDirectory() + $"/wwwroot/images/{id}{ext}";
            using (var stream = System.IO.File.Create(filePath))
            {
                await file.CopyToAsync(stream);
            }
            var aplication = new Application()
            {
                PostId = postId,
                CV = $"{id}{ext}",
                ApplicantId = 1,
                Status = true,
            };
            await unitOfWork.ApplicationRepository.Add(aplication);
            return Ok(true);
        }
    }
}
