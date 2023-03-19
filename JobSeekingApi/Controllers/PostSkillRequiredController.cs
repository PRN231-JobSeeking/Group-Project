using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppCore;
using AppCore.Models;
using AppRepository.Repositories;
using ClientRepository.Models;
using Microsoft.AspNetCore.Authorization;
using AppRepository.UnitOfWork;
using System.Diagnostics;

namespace JobSeekingApi.Controllers
{
    [Route("api/PostSkill")]
    [ApiController]
    public class PostSkillRequiredController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public PostSkillRequiredController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        [HttpGet("{postId}")]
        public async Task<IActionResult> GetPostSkill(int postId)
        {
            var postSkills = await _unitOfWork.PostSkillRepository.Get(ps => ps.PostId == postId);

            if (postSkills == null || postSkills.Count() == 0)
            {
                return NotFound();
            }
            var postSkillModels = new List<PostSkillModel>();
            foreach(var postSkill in postSkills)
            {
                var skill = await _unitOfWork.SkillRepository.GetFirst(s => s.Id == postSkill.SkillId);
                if(skill != null)
                {
                    postSkillModels.Add(new PostSkillModel()
                    {
                        SkillId = postSkill.SkillId,
                        SkillName = skill.Name,
                        PostId = postId,
                    });
                }
            }
            if(postSkillModels.Count == 0)
            {
                return BadRequest();
            }
            return Ok(postSkillModels);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostSkill(PostSkillModel postSkillModel)
        {
            var postSkillInDb = await _unitOfWork.PostSkillRepository.GetFirst(ps => ps.SkillId == postSkillModel.SkillId && ps.PostId == postSkillModel.PostId
                                                                && ps.IsDeleted == false);
            if(postSkillInDb == null)
            {
                await _unitOfWork.PostSkillRepository.Add(new PostSkillRequired()
                {
                    PostId = postSkillModel.PostId,
                    SkillId = postSkillModel.SkillId,
                    IsDeleted = false,
                });
                return Ok();
            }
            return BadRequest("Already exist post skill!");
        }
    }
}
