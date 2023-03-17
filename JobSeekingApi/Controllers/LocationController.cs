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
    public class LocationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public LocationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetLocations()
        {
            var result = await _unitOfWork.LocationRepository.Get();
            if (result == null || result.Count() == 0)
            {
                return NotFound("List was empty!");
            }
            return Ok(result);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLocations(int id)
        {
            var find = await _unitOfWork.LocationRepository.Get(location => location.Id == id);
            var level = find.FirstOrDefault();
            if (level == null)
            {
                return NotFound();
            }
            return Ok(level);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocation(LocationModel location)
        {
            var list = await _unitOfWork.LocationRepository.Get(l => l.Id == location.Id);
            if (list == null || list.Count() == 0)
            {
                return BadRequest();
            }
            var locationInDb = await _unitOfWork.LocationRepository.GetFirst(c => c.Name.ToLower().Equals(location.Name.ToLower()));
            if (locationInDb != null)
            {
                return BadRequest("Already exist location name!");
            }
            locationInDb = list.FirstOrDefault();
            if (locationInDb != null)
            {
                locationInDb.Name = location.Name;
                locationInDb.IsDeleted = location.IsDeleted;
                await _unitOfWork.LocationRepository.Update(locationInDb);
                return Ok(true);
            }
            return NotFound("Location Id not exist!");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostLocation(LocationModel location)
        {
            var list = await _unitOfWork.LocationRepository.Get(l => l.Name.ToLower().Equals(location.Name.ToLower()));
            if (list != null)
            {
                var lInDb = list.FirstOrDefault();
                if (lInDb != null)
                {
                    return BadRequest("Already exist location name!");
                }
            }
            await _unitOfWork.LocationRepository.Add(new Location()
            {
                IsDeleted = location.IsDeleted,
                Name = location.Name,
            });
            return Ok(true);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            var find = await _unitOfWork.LocationRepository.Get(location => location.Id == id);
            var location = find.FirstOrDefault();
            if (location == null)
            {
                return NotFound("Not Exist Location Id!");
            }

            await _unitOfWork.LocationRepository.Delete(location);
            return Ok(true);
        }
    }
}
