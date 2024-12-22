using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimoServer.Models;
using MinimoShared;

namespace MinimoServer.Controllers;

    // accountsController for handling account API requests
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly GameDbContext _context;

        public AccountsController(GameDbContext context)
        {
            _context = context;
        }

        // Get all accounts (returns AccountDTOs)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountDTO>>> GetAccounts()
        {
            var accounts = await _context.Accounts.ToListAsync();
            return Ok(AccountMapper.ToAccountDTOs(accounts));  // Use the accountMapper to convert
        }

        // Get account by ID (returns AccountDTO)
        //[Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDTO>> GetAccount(int id)
        {
            var account = await _context.Accounts.FindAsync(id);

            if (account == null)
            {
                return NotFound(new { message = "Account not found" });
            }

            return Ok(AccountMapper.ToAccountDTO(account));  // Use the AccountMapper to convert
        }

        // Create a new account using AccountDTO
        [HttpPost]
        public async Task<ActionResult<AccountDTO>> CreateAccount(CreateAccountDTO createAccountDto)
        {
            // Check if the account already exists
            if (await _context.Accounts.AnyAsync(a => a.Username == createAccountDto.Username))
            {
                return Conflict(new { message = "Account already exists" });
            }
            
            var account = new Account
            {
                Username = createAccountDto.Username,
                Password = createAccountDto.Password,
                Nickname = createAccountDto.Nickname,
                Level = 1,
                Experience = 0,
                CreatedAt = DateTime.UtcNow
            };
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            var accountDto = AccountMapper.ToAccountDTO(account);  // Use the accountMapper to convert
            return CreatedAtAction(nameof(GetAccount), new { id = accountDto.ID }, accountDto);
        }

        // Update nickname
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNickname(int id, string newNickname)
        {
            // Find the account by ID
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound(new { message = "account not found" });
            }
            
            if (string.IsNullOrWhiteSpace(newNickname))
            {
                return BadRequest(new { message = "Nickname cannot be empty" });
            }
            
            if (newNickname.Length > 20)
            {
                return BadRequest(new { message = "Nickname is too long" });
            }
            
            if (newNickname == account.Nickname)
            {
                return BadRequest(new { message = "Nickname is the same" });
            }

            account.Nickname = newNickname;
            await _context.SaveChangesAsync();
            
            return Ok(new { message = "Nickname updated" });
        }

        // Delete an account
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deleteaccount(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound(new { message = "account not found" });
            }

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();

            return Ok(new { message = "account deleted" });
        }
    }