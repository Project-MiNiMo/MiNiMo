using Microsoft.AspNetCore.Mvc;
using MinimoServer.Models;
using MinimoServer.Services;

namespace MinimoServer.Controllers;

[ApiController]
public abstract class BaseController : ControllerBase
{
    protected readonly GameDbContext _context;
    protected readonly TimeService _timeService;
    protected readonly TableDataService _tableDataService;
    
    protected BaseController(GameDbContext context, TimeService timeService, TableDataService tableDataService)
    {
        _context = context;
        _timeService = timeService;
        _tableDataService = tableDataService;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected int? GetAccountIdFromClaims()
    {
        var accountClaim = User.Claims.FirstOrDefault(c => c.Type == "AccountId");
        return accountClaim != null && int.TryParse(accountClaim.Value, out var accountId) 
            ? accountId 
            : null;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns></returns>
    protected async Task<Account?> GetAccountAsync(int accountId) =>
        await _context.Accounts.FindAsync(accountId);
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected async Task<Account?> GetAuthorizedAccountAsync()
    {
        var accountId = GetAccountIdFromClaims();
        if (accountId == null) return null;

        return await GetAccountAsync(accountId.Value);
    }
}