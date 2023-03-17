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
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var result = await _unitOfWork.CategoryRepository.Get();
            if (result == null || result.Count() == 0)
            {
                return NotFound("List was empty!");
            }
            return Ok(result);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategories(int id)
        {
            var find = await _unitOfWork.CategoryRepository.Get(category => category.Id == id);
            var category = find.FirstOrDefault();
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(CategoryModel category)
        {
            var list = await _unitOfWork.CategoryRepository.Get(c => c.Id == category.Id);
            if (list == null || list.Count() == 0)
            {
                return BadRequest();
            }
            var categoryInDb = await _unitOfWork.CategoryRepository.GetFirst(c => c.Name.ToLower().Equals(category.Name.ToLower()));
            if (categoryInDb != null)
            {
                return BadRequest("Already exist category name!");
            }
            var locationInDb = list.FirstOrDefault();
            if (locationInDb != null)
            {
                locationInDb.Name = category.Name;
                locationInDb.IsDeleted = category.IsDeleted;
                await _unitOfWork.CategoryRepository.Update(locationInDb);
                return Ok(true);
            }
            return NotFound("Category Id not exist!");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostCategory(CategoryModel category)
        {
            var list = await _unitOfWork.CategoryRepository.Get(c => c.Name.ToLower().Equals(category.Name.ToLower()));
            if (list != null)
            {
                var lInDb = list.FirstOrDefault();
                if (lInDb != null)
                {
                    return BadRequest("Already exist category name!");
                }
            }
            await _unitOfWork.CategoryRepository.Add(new Category()
            {
                IsDeleted = category.IsDeleted,
                Name = category.Name,
            });
            return Ok(true);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var find = await _unitOfWork.CategoryRepository.Get(category => category.Id == id);
            var category = find.FirstOrDefault();
            if (category == null)
            {
                return NotFound("Not Exist Category Id!");
            }

            await _unitOfWork.CategoryRepository.Delete(category);
            return Ok(true);
        }
    }
}
