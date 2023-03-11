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
using ClientRepository.Models;

namespace JobSeekingApi.Controllers
{
    [Route("api/interviews")]
    [ApiController]
    public class InterviewController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public InterviewController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Interview
       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Interview>>> GetInterviews()
        {
            return Ok(await _unitOfWork.InterviewRepository.Get(expression: null, "Application", "Interviewer", "Slot"));
        }
        [HttpGet("create/available/{applicationId}")]
        public async Task<IActionResult> GetAvailableInterviewers([FromQuery]int slotId, [FromQuery]string date, [FromRoute]int applicationId) { 
            var list = await _unitOfWork.InterviewRepository.GetAvailableInterviewers(slotId, DateOnly.Parse(date), applicationId);
            return Ok(list);
        }
        // GET: api/Interview/5
        [HttpGet("application/{application_id}")]
        public async Task<ActionResult<Interview>> GetInterviewOnApplication(int application_id)
        {
            var interviews = await _unitOfWork.InterviewRepository.Get(c => c.ApplicationId == application_id);
            return Ok(interviews);
        }

        // PUT: api/Interview/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInterview(int id, Interview interview)
        {
            if (id != interview.ApplicationId)
            {
                return BadRequest();
            }            

            try
            {
                await _unitOfWork.InterviewRepository.Update(interview);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Interview
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Interview>> PostInterview(Interview interview)
        {            
            try
            {
                await _unitOfWork.InterviewRepository.CreateMeeting(interview);
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return CreatedAtAction("GetInterview", new { id = interview.ApplicationId }, interview);
        }

        // DELETE: api/Interview/5
        [HttpDelete("application/{application_id}/{round_id}")]
        public async Task<IActionResult> DeleteInterview(int application_id, int round_id)
        {
            var interview = await _unitOfWork.InterviewRepository.GetFirst(c => c.ApplicationId == application_id && c.Round == round_id);
            if (interview == null)
            {
                return NotFound();
            }

            await _unitOfWork.InterviewRepository.Delete(interview);            

            return NoContent();
        }

        [Authorize]
        [HttpPut("feedback")]
        public async Task<IActionResult> PutInterviewFeedback(InterviewFeedbackModel interviewFeedback)
        {
            var interviews = await _unitOfWork.InterviewRepository.Get(i => i.InterviewerId == interviewFeedback.InterviewerId && i.Round == interviewFeedback.Round 
                                                    && i.ApplicationId == interviewFeedback.ApplicationId);
            if(interviews == null || interviews.Count() == 0)
            {
                return NotFound("Not Exist Interview!");
            }
            var interview = interviews.FirstOrDefault();
            if(interview == null)
            {
                return NotFound("Not Exist Interview!");
            }
            if(interviewFeedback.Point > 100 || interviewFeedback.Point < 0)
            {
                return BadRequest("Point must in range >= 0 and 100 <= ");
            }
            interview.Feedback = interviewFeedback.Feedback;
            interview.Point = interviewFeedback.Point;
            try
            {
                await _unitOfWork.InterviewRepository.Update(interview);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return Ok(true);
        }

        [Authorize]
        [HttpGet("Nonfeedbacks/InterviewerId/{interviewerId}")]
        public async Task<IActionResult> GetAllNonFeedbacks(int interviewerId)
        {
            var interviews = await _unitOfWork.InterviewRepository.Get(i => i.IsDeleted == false && i.Point == 0
                                                                        && string.IsNullOrEmpty(i.Feedback)
                                                                        && i.InterviewerId == interviewerId);
            if(interviews == null || interviews.Count() == 0)
            {
                return BadRequest();
            }
            return Ok(interviews);
        }

        [Authorize]
        [HttpGet("Nonfeedbacks/InterviewerId/{interviewerId}/SlotId/{slotId}")]
        public async Task<IActionResult> GetAllNonFeedbacksWithSlot(int interviewerId, int slotId)
        {
            var interviews = await _unitOfWork.InterviewRepository.Get(i => i.IsDeleted == false && i.Point == 0
                                                                        && string.IsNullOrEmpty(i.Feedback)
                                                                        && i.InterviewerId == interviewerId
                                                                        && i.SlotId == slotId);
            if (interviews == null || interviews.Count() == 0)
            {
                return BadRequest();
            }
            return Ok(interviews);
        }

        [Authorize]
        [HttpGet("Nonfeedbacks/InterviewerId/{interviewerId}/ApplicantId/{applicantId}")]
        public async Task<IActionResult> GetInterviewNonFeedback(int interviewerId, int applicantId)
        {
            var interview = await _unitOfWork.InterviewRepository.GetFirst(i => i.IsDeleted == false && i.Point == 0
                                                                        && string.IsNullOrEmpty(i.Feedback)
                                                                        && i.InterviewerId == interviewerId
                                                                        && i.ApplicationId == applicantId);
            if (interview == null)
            {
                return BadRequest();
            }
            return Ok(interview);
        }

    }
}
