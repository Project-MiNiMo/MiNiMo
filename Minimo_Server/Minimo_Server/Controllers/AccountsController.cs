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

        /// <summary>
        /// 모든 계정 목록을 반환합니다.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountDTO>>> GetAccounts()
        {
            var accounts = await _context.Accounts.ToListAsync();
            return Ok(AccountMapper.ToAccountDTOs(accounts));  // Use the accountMapper to convert
        }

        /// <summary>
        /// 주어진 index에 해당하는 계정을 반환합니다.
        /// </summary>
        /// <param name="index">계정 index</param>
        /// <returns></returns>
        //[Authorize]
        [HttpGet("{index}")]
        public async Task<ActionResult<AccountDTO>> GetAccount(int index)
        {
            var account = await _context.Accounts.FindAsync(index);

            if (account == null)
            {
                return NotFound(new { message = "Account not found" });
            }

            return Ok(AccountMapper.ToAccountDTO(account));  // Use the AccountMapper to convert
        }

        /// <summary>
        /// 계정을 생성하고, 생성된 계정을 반환합니다.
        /// TODO: 이미 존재하는 계정인지 확인
        /// </summary>
        /// <param name="createAccountDto">CreateAccountDTO</param>
        /// <returns>AccountDTO</returns>
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
            return Ok(accountDto);
        }

        /// <summary>
        /// 계정의 닉네임을 변경합니다.
        /// </summary>
        /// <param name="newNickname">변경할 닉네임</param>
        /// <returns></returns>
        [HttpPut("{index}")]
        public async Task<IActionResult> UpdateNickname(string newNickname)
        {
            var accountIdClaim = User.Claims.FirstOrDefault(c => c.Type == "AccountId");
            if (accountIdClaim == null)
            {
                return Unauthorized("AccountId claim not found");
            }
            
            if (!int.TryParse(accountIdClaim.Value, out var index))
            {
                return Unauthorized("AccountId claim is not an integer");
            }
            
            // Find the account by ID
            var account = await _context.Accounts.FindAsync(index);
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

        /// <summary>
        /// 요청한 계정을 삭제합니다.
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteAccount()
        {
            var accountIdClaim = User.Claims.FirstOrDefault(c => c.Type == "AccountId");
            if (accountIdClaim == null)
            {
                return Unauthorized("AccountId claim not found");
            }
            
            if (!int.TryParse(accountIdClaim.Value, out var index))
            {
                return Unauthorized("AccountId claim is not an integer");
            }
            
            var account = await _context.Accounts.FindAsync(index);
            if (account == null)
            {
                return NotFound(new { message = "account not found" });
            }

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();

            return Ok(new { message = "account deleted" });
        }
    }