using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppCore;
using AppCore.Models;
using AppRepository.UnitOfWork;
using Microsoft.AspNetCore.Authorization;

namespace JobSeekingApi.Controllers
{
    [Route("api/hr-delete-post")]
    [ApiController]
    public class HrPostDeleteController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public HrPostDeleteController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/HrPostDelete/5
        [Authorize(Roles = "HR")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            var post = await _unitOfWork.PostRepository.GetFirst(c=>c.Id == id, "Category", "Location", "Level");
            if (post == null)
            {
                return NotFound();
            }
            return post;
        }

        // DELETE: api/HrPostDelete/5
        [Authorize(Roles ="HR")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _unitOfWork.PostRepository.GetFirst(c => c.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            //check if any applicant is going to be interviewed within that day
                //get applications in that post within that day
            var applications = await _unitOfWork.ApplicationRepository.Get(c => c.PostId == id);
            foreach (var item in applications)
            {
                var interviews = await _unitOfWork.InterviewRepository.Get(c => c.ApplicationId == item.Id && c.Date.Equals(DateTime.Today));
                if(interviews.Any())
                {
                    //there is/are interview(s) within today
                    return BadRequest("Some interviews are currently in progress today!");
                }
            }
            await _unitOfWork.PostRepository.Delete(post);
            //after delete -> set status of other applicants 
            //pass -> pass
            //pending -> fail
            //fail -> fail
            foreach (var item in applications)
            {
                if(item.Status == null)
                {
                    item.Status = false;
                    await _unitOfWork.ApplicationRepository.Update(item);
                }
            }
            return NoContent();
        }

        
    }
}
