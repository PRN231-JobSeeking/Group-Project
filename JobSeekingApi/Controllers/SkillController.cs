using AppCore.Models;
using AppRepository.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace JobSeekingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public SkillController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Role
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Skill>>> GetSkills()
        {
            var result = await _unitOfWork.SkillRepository.Get();
            return Ok(result);
        }

        // GET: api/Role/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Skill>> GetSkills(int id)
        {
            var result = await _unitOfWork.SkillRepository.Get(r => r.Id == id);
            var role = result.FirstOrDefault();
            if (role == null)
            {
                return NotFound();
            }

            return role;
        }

        // PUT: api/Role/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSkill(int id, Skill Skill)
        {
            if (id != Skill.Id)
            {
                return BadRequest();
            }
            await _unitOfWork.SkillRepository.Update(Skill);

            return NoContent();
        }

        // POST: api/Role
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Skill>> PostRole(Skill Skill)
        {
            await _unitOfWork.SkillRepository.Add(Skill);
            return CreatedAtAction("GetRole", new { id = Skill.Id }, Skill);
        }

        // DELETE: api/Role/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var result = await _unitOfWork.SkillRepository.Get(r => r.Id == id);
            var Skill = result.FirstOrDefault();
            if (Skill == null)
            {
                return NotFound();
            };

            await _unitOfWork.SkillRepository.Delete(Skill);

            return NoContent();
        }
    }
}
