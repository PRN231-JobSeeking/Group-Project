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

namespace JobSeekingApi.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(IUnitOfWork unitOfWork)
        {
            _unitOfWork= unitOfWork;
        }

        // GET: api/Account
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
        {
            var result =  await _unitOfWork.AccountRepository.Get();
            return Ok(result);
        }

        // GET: api/Account/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(int id)
        {
            var find = await _unitOfWork.AccountRepository.Get(account => account.Id == id);
            var account = find.FirstOrDefault();
            if (account == null)
            {
                return NotFound();
            }

            return account;
        }

        //[HttpPost]
        //[Route("Register")]
        //public async Task<ActionResult<Account>> CreateAccount([FromBody] RegisterModel model)
        //{
        //    var account = new Account()
        //    {
        //        Email= model.Email,
        //        FirstName= model.FirstName,
        //        LastName= model.LastName,
        //        Address= model.Address,
        //        Phone= model.Phone,
        //        RoleId=2                                        
        //    };
        //    try
        //    {
        //        await _unitOfWork.AccountRepository.Add(account);

        //        return Ok(account);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError,
        //            ex.Message
        //            );
        //    }
        //}

        // PUT: api/Account/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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
