using AppCore.Models;
using AppRepository.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace JobSeekingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserSkillController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserSkillController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        // POST: api/Role
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserSkill>> PostSkill(UserSkill Skill)
        {
            await _unitOfWork.UserSkillRepository.Add(Skill);
            return Ok(Skill);
        }

       
    }
}
