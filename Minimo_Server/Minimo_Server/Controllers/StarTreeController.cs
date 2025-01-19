using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinimoServer.Services;
using MinimoShared;

namespace MinimoServer.Controllers;

public class StarTreeController(GameDbContext context, TimeService timeService, TableDataService tableDataService)
    : BaseController(context, timeService, tableDataService)
{
    /// <summary>
    /// 빛나는 별나무를 수확합니다.
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<StarTreeResultDTO>> GetStarTreeResult()
    {
        var account = await GetAuthorizedAccountAsync();
        if (account == null) return Unauthorized("Account not found");

        var currentTime = _timeService.CurrentTime;
        var lastStarTreeTime = account.LastStarTreeCreatedAt;
        var hours = (currentTime - lastStarTreeTime).Hours;
        if (hours < 1)
        {
            return BadRequest("StarTree is not ready yet");
        }
        
        var starTreeTableData = _tableDataService.StarTree[account.StarTreeLevel];
        var maxHours = starTreeTableData.LimitTime / 3600;
        if (hours > maxHours)
        {
            hours = maxHours;
        }

        account.Currency.Star += starTreeTableData.StarCoin * hours;
        account.Currency.HPI += starTreeTableData.HPI * hours;
        account.Experience += starTreeTableData.EXP * hours;
        
        account.LastStarTreeCreatedAt = currentTime;
        await _context.SaveChangesAsync();
        
        return Ok(new StarTreeResultDTO
        {
            UpdatedCurrency = CurrencyMapper.ToCurrencyDTO(account.Currency),
        });
    }

    /// <summary>
    /// 소원을 빌어 별나무의 최대 보상을 획득합니다.
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet("wish")]
    public async Task<ActionResult<StarTreeResultDTO>> GetWishResult()
    {
        var account = await GetAuthorizedAccountAsync();
        if (account == null) return Unauthorized("Account not found");
        
        // 이미 소원을 빈 경우
        if (account.LastWishedAt.Date == _timeService.CurrentTime.Date)
        {
            return BadRequest("Wish is already done");
        }
        
        var starTreeTableData = _tableDataService.StarTree[account.StarTreeLevel];
        var maxHours = starTreeTableData.LimitTime / 3600;
        
        account.Currency.Star += starTreeTableData.StarCoin * maxHours;
        account.Currency.HPI += starTreeTableData.HPI * maxHours;
        account.Experience += starTreeTableData.EXP * maxHours;
        
        account.LastWishedAt = _timeService.CurrentTime;
        await _context.SaveChangesAsync();
        
        return Ok(new StarTreeResultDTO
        {
            UpdatedCurrency = CurrencyMapper.ToCurrencyDTO(account.Currency),
        });
    }

    /// <summary>
    /// 별나무 레벨을 1 증가시킵니다.
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    public async Task<ActionResult<int>> LevelUp()
    {
        var account = await GetAuthorizedAccountAsync();
        if (account == null) return Unauthorized("Account not found");

        // TODO : 별나무 레벨업 비용
        // if (account.Currency.Star < _tableDataService.StarTree[account.StarTreeLevel].LevelUpCost)
        // {
        //     return BadRequest("Not enough Star");
        // }
        //
        // account.Currency.Star -= _tableDataService.StarTree[account.StarTreeLevel].LevelUpCost;
        account.StarTreeLevel++;
        await _context.SaveChangesAsync();

        return Ok(account.StarTreeLevel);
    }
}