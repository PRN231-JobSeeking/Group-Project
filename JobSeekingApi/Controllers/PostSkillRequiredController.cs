using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppCore;
using AppCore.Models;

namespace JobSeekingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostSkillRequiredController : ControllerBase
    {
        private readonly Context _context;

        public PostSkillRequiredController(Context context)
        {
            _context = context;
        }

        // GET: api/PostSkillRequired
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostSkillRequired>>> GetPostSkills()
        {
            return await _context.PostSkills.ToListAsync();
        }

        // GET: api/PostSkillRequired/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PostSkillRequired>> GetPostSkillRequired(int id)
        {
            var postSkillRequired = await _context.PostSkills.FindAsync(id);

            if (postSkillRequired == null)
            {
                return NotFound();
            }

            return postSkillRequired;
        }

        // PUT: api/PostSkillRequired/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPostSkillRequired(int id, PostSkillRequired postSkillRequired)
        {
            if (id != postSkillRequired.SkillId)
            {
                return BadRequest();
            }

            _context.Entry(postSkillRequired).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostSkillRequiredExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/PostSkillRequired
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PostSkillRequired>> PostPostSkillRequired(PostSkillRequired postSkillRequired)
        {
            _context.PostSkills.Add(postSkillRequired);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PostSkillRequiredExists(postSkillRequired.SkillId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPostSkillRequired", new { id = postSkillRequired.SkillId }, postSkillRequired);
        }

        // DELETE: api/PostSkillRequired/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePostSkillRequired(int id)
        {
            var postSkillRequired = await _context.PostSkills.FindAsync(id);
            if (postSkillRequired == null)
            {
                return NotFound();
            }

            _context.PostSkills.Remove(postSkillRequired);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PostSkillRequiredExists(int id)
        {
            return _context.PostSkills.Any(e => e.SkillId == id);
        }
    }
}
