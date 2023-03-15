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
using ClientRepository.Models;
using Microsoft.AspNetCore.Authorization;

namespace JobSeekingApi.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Account
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
        {
            var result =  await _unitOfWork.AccountRepository.Get(includeProperties: "Role");
            return Ok(result);
        }

        // GET: api/Account/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(int id)
        {
            var find = await _unitOfWork.AccountRepository.Get(account => account.Id == id,"Role","UserSkill");
            var account = find.FirstOrDefault();
            if (account == null)
            {
                return NotFound();
            }

            return account;
        }


        // PUT: api/Account/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccount(int id, Account account)
        {
            if (id != account.Id)
            {
                return BadRequest();
            }

            await _unitOfWork.AccountRepository.Update(account);
            return StatusCode(StatusCodes.Status200OK);
        }

        // POST: api/Account
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Account>> PostAccount(Account account)
        {
            await _unitOfWork.AccountRepository.Add(account);
            return CreatedAtAction("GetAccount", new { id = account.Id }, account);
        }

        // DELETE: api/Account/5
        [Authorize(Roles= "Administrator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var find = await _unitOfWork.AccountRepository.Get(account => account.Id == id);
            var account = find.FirstOrDefault();
            if (account == null)
            {
                return NotFound();
            }

            await _unitOfWork.AccountRepository.Delete(account);
            return NoContent();
        }


    }

}
