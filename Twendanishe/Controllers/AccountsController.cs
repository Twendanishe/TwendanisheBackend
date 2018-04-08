using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Twendanishe.Business;
using Twendanishe.Data;
using Twendanishe.Models;
using Twendanishe.ViewModels;

namespace Twendanishe.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/Accounts")]
    public class AccountsController : Controller
    {
        private readonly TwendanisheContext _context;
        private readonly IConfiguration _configuration;

        #region Business

        private readonly AccountService _accountService;

        #endregion

        public AccountsController(
            TwendanisheContext context, IConfiguration configuration, 
            AccountService accountService)
        {
            _context = context;
            _configuration = configuration;
            _accountService = accountService;
        }

        // GET: api/Accounts
        [HttpGet]
        public IEnumerable<User> GetUsers()
        {
            return _context.Users;
        }

        // GET: api/Accounts/profile/5
        [HttpGet("profile/{id}")]
        public async Task<IActionResult> GetUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users.SingleOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Accounts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser([FromRoute] int id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Accounts
        [HttpPost]
        public async Task<IActionResult> PostUser([FromBody] UserViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newUser = await _accountService.Create(user);

            if (!newUser) return BadRequest();
            return Ok(new { status = "success" });
        }

        // DELETE: api/Accounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users.SingleOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        // POST: api/Accounts/pay
        [HttpPost]
        [Route("pay")]
        public async Task<IActionResult> PostPayment([FromBody] PaymentViewModel payment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var payItem = await _accountService.Pay(payment);

            if (payItem)
            {
                return Ok(new
                {
                    status = "success"
                });
            }
            else
            {
                return BadRequest();
            }
        }

        // POST: api/Accounts/reset
        [HttpPost]
        [Route("reset")]
        public async Task<IActionResult> PostReset([FromBody] EmailResetViewModel emailReset)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users.Where(u => u.Email == emailReset.Email).FirstOrDefaultAsync();

            if (user != null)
            {
                return Ok(new
                {
                    status = "success",
                    message = "Successfully reset password. Check your email for instructions!"
                });
            }
            else
            {
                return BadRequest();
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("token")]
        public async Task<IActionResult> RequestToken([FromBody] LoginViewModel request)
        {
            if (await _accountService.Login(request))
            {
                var claims = new[] { new Claim(ClaimTypes.Name, request.UserName) };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("SecurityKey")));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration.GetValue<string>("TokenIssuer"),
                    audience: _configuration.GetValue<string>("TokenAudience"),
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }

            return BadRequest("Could not verify username and password");
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}