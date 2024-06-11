using System;
using System.IdentityModel.Tokens.Jwt;
using API.Dto;
using API.Repository;
using API.Utils;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IConfiguration _configuration;

        public AccountController(IAccountRepository accountRepository, IConfiguration configuration)
        {
            _accountRepository = accountRepository;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto userLogin)
        {
            // Check admin credentials from appsettings.json
            var adminEmail = _configuration["DefaultAdmin:Email"];
            var adminPassword = _configuration["DefaultAdmin:Password"];

            if (userLogin.Email == adminEmail && userLogin.Password == adminPassword)
            {
                var adminAccount = new SystemAccount()
                {
                    AccountEmail = adminEmail,
                    AccountPassword = adminPassword,
                    AccountRole = 0
                };
                var token = helper.GenerateJwtToken(adminAccount,_configuration);
                return Ok(new { token });
            }
            var account = await _accountRepository.GetAccountByEmailAsync(userLogin.Email);
            if (account == null || account.AccountPassword != userLogin.Password)
            {
                return Unauthorized("Invalid credentials");
            }

            var userToken = helper.GenerateJwtToken(account, _configuration);
            return Ok(new { token = userToken });
        }

        

        [Authorize(Roles = "0")] // Admin only
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SystemAccount>>> GetAccounts()
        {
            return Ok(await _accountRepository.GetAccountsAsync());
        }

        [Authorize(Roles = "0")] // Admin only
        [HttpGet("{id}")]
        public async Task<ActionResult<SystemAccount>> GetAccount(int id)
        {
            var account = await _accountRepository.GetAccountAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return Ok(account);
        }

        [Authorize(Roles = "0")] // Admin only
        [HttpPost]
        public async Task<ActionResult<SystemAccount>> CreateAccount(SystemAccount account)
        {
            await _accountRepository.AddAccountAsync(account);
            return CreatedAtAction(nameof(GetAccount), new { id = account.AccountId }, account);
        }

        [Authorize(Roles = "0")] // Admin only
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(int id, SystemAccount account)
        {
            if (id != account.AccountId)
            {
                return BadRequest();
            }

            await _accountRepository.UpdateAccountAsync(account);
            return NoContent();
        }

        [Authorize(Roles = "0")] // Admin only
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var account = await _accountRepository.GetAccountAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            // Check if account is associated with any news articles
            if (await _accountRepository.HasNewsArticlesAsync(id))
            {
                return BadRequest("Account cannot be deleted as it is associated with news articles.");
            }

            await _accountRepository.DeleteAccountAsync(id);
            return NoContent();
        }
    }
}