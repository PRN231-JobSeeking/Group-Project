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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserSkill>>> GetAccounts()
        {
            var result = await _unitOfWork.UserSkillRepository.Get();
            return Ok(result);
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
