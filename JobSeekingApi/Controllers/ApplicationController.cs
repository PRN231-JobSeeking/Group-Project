using AppCore.Models;
using AppRepository.UnitOfWork;
using ClientRepository.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace JobSeekingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IConfiguration _config;

        public ApplicationController(IConfiguration config, IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            _config = config;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetApplicationList()
        {
            var aplication = unitOfWork.ApplicationRepository.Get().Result;
            return Ok(aplication);
        }

        [Authorize]
        [HttpGet]
        [Route("Get/Id/{aplicationId}")]
        public async Task<IActionResult> GetApplication([FromRoute] int aplicationId)
        {
            var aplication = unitOfWork.ApplicationRepository.Get(a => a.Id == aplicationId).Result.FirstOrDefault();
            if (aplication == null)
            {
                return NotFound("Not found aplicationId!");
            }
            return Ok(aplication);
        }

        [Authorize]
        [HttpGet]
        [Route("ApplicationNonInterview/ApplicantId/{aplicantId}")]
        public async Task<IActionResult> GetApplicationByApplicant([FromRoute] int aplicantId)
        {
            var aplication = await unitOfWork.ApplicationRepository.Get(a => a.ApplicantId == aplicantId && a.IsDeleted == false
                                                                                && a.Status == null);
            if (aplication == null)
            {
                return NotFound("Not found aplicationId!");
            }
            return Ok(aplication);
        }

        [Authorize]
        [HttpGet]
        [Route("Get/ApplicantId/{aplicantId}/PostId/{postId}")]
        public async Task<IActionResult> GetApplication([FromRoute] int aplicantId, [FromRoute] int postId)
        {
            var aplications = await unitOfWork.ApplicationRepository.Get(a => a.ApplicantId == aplicantId && a.PostId == postId 
                                                                               && a.IsDeleted == false);
            if (aplications == null)
            {
                return NotFound("Not found aplicationId!");
            }
            return Ok(aplications);
        }

        [Authorize]
        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<bool>> Create([FromForm] int postId,[FromForm] int applicationId, IFormFile file)
        {
            if (postId <= 0 || file == null)
            {
                return BadRequest();
            }
            var postIdInDb = unitOfWork.PostRepository.Get(p => p.Id == postId).Result.FirstOrDefault();
            if (postIdInDb == null)
            {
                return NotFound("Not found post id!");
            }
            string ext = Path.GetExtension(file.FileName);
            if (!ext.Equals(".pdf"))
            {
                return NotFound("File must have extension .pdf");
            }
            string id = Guid.NewGuid().ToString();
            string directory = _config["ApplicationRoot:Directory"];
            string folder = _config["ApplicationRoot:Folder"];
            if(string.IsNullOrEmpty(directory) || string.IsNullOrEmpty(folder))
            {
                return NotFound("Appsettings need to config Application Root!");
            }
            if (!Directory.Exists(Directory.GetCurrentDirectory() + $"/{directory}"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + $"/{directory}");
            }
            if (!Directory.Exists(Directory.GetCurrentDirectory() + $"/{directory}/{folder}"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + $"/{directory}/{folder}");
            }
            string filePath = Directory.GetCurrentDirectory() + $"/{directory}/{folder}/{id}{ext}";
            using (var stream = System.IO.File.Create(filePath))
            {
                await file.CopyToAsync(stream);
            }
            var application = new Application()
            {
                PostId = postId,
                CV = $"{id}{ext}",
                ApplicantId = applicationId,
                Status = null,
            };
            await unitOfWork.ApplicationRepository.Add(application);
            return Ok(true);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApplication(int id, ApplicationModel application)
        {
            if (id != application.Id)
            {
                return BadRequest();
            }
            var applicationInDb = await unitOfWork.ApplicationRepository.GetFirst(a => a.Id == application.Id);
            if(applicationInDb == null)
            {
                return BadRequest();
            }
            applicationInDb.Status = application.Status;
            applicationInDb.CV = application.CV;
            applicationInDb.ApplicantId = application.ApplicantId;
            applicationInDb.IsDeleted = application.IsDeleted;
            applicationInDb.PostId = application.PostId;

            await unitOfWork.ApplicationRepository.Update(applicationInDb);
            return StatusCode(StatusCodes.Status200OK);
        }
    }
}
