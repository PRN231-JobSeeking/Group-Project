using AppCore.Models;
using AppRepository.UnitOfWork;
using ClientRepository.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobSeekingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LevelController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public LevelController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetLevels()
        {
            var result = await _unitOfWork.LevelRepository.Get();
            if(result == null || result.Count() == 0)
            {
                return NotFound("List was empty!");
            }
            return Ok(result);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLevels(int id)
        {
            var find = await _unitOfWork.LevelRepository.Get(level => level.Id == id);
            var level = find.FirstOrDefault();
            if (level == null)
            {
                return NotFound();
            }
            return Ok(level);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLevel(LevelModel level)
        {
            var list = await _unitOfWork.LevelRepository.Get(l => l.Id == level.Id);
            if(list == null || list.Count() == 0)
            {
                return BadRequest();
            }
            var levelInDb = await _unitOfWork.LevelRepository.GetFirst(c => c.Name.ToLower().Equals(level.Name.ToLower()));
            if (levelInDb != null)
            {
                return BadRequest("Already exist level name!");
            }
            levelInDb = list.FirstOrDefault();
            if (levelInDb != null)
            {
                levelInDb.Name = level.Name;
                levelInDb.IsDeleted = level.IsDeleted;
                await _unitOfWork.LevelRepository.Update(levelInDb);
                return Ok(true);
            }
            return NotFound("Level Id not exist!");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostAccount(LevelModel level)
        {
            var list = await _unitOfWork.LevelRepository.Get(l => l.Name.ToLower().Equals(level.Name.ToLower()));
            if(list != null)
            {
                var lInDb = list.FirstOrDefault();
                if(lInDb != null)
                {
                    return BadRequest("Already exist level name!");
                }
            }
            await _unitOfWork.LevelRepository.Add(new Level()
            {
                IsDeleted = level.IsDeleted,
                Name = level.Name,
            });
            return Ok(true);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLevel(int id)
        {
            var find = await _unitOfWork.LevelRepository.Get(level => level.Id == id);
            var level = find.FirstOrDefault();
            if (level == null)
            {
                return NotFound("Not Exist Level Id!");
            }

            await _unitOfWork.LevelRepository.Delete(level);
            return Ok(true);
        }
    }
}
