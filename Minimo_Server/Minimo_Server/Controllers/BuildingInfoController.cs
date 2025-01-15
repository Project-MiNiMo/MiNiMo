using System.Numerics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimoServer.Models;
using MinimoServer.Services;
using MinimoShared;

namespace MinimoServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BuildingInfoController(GameDbContext context, TimeService timeService, TableDataService tableDataService) : BaseController(context, timeService, tableDataService)
{
    /// <summary>
    /// 건물 해금(최초)을 진행합니다.
    /// 만약 이미 건물이 있거나 행복도가 충분하지 않으면 오류를 반환합니다.
    /// </summary>
    /// <param name="buildingType"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<BuildingInfoDTO>> CreateBuildingInfo(string buildingType)
    {
        var account = await GetAuthorizedAccountAsync();
        if (account == null) return Unauthorized("Account not found");
        
        // 건물 타입이 유효하지 않으면 오류
        if (!_tableDataService.Building.TryGetValue(buildingType, out Data.Class.BuildingData? value))
        {
            return BadRequest("Invalid building type");
        }
        
        // 만약 이미 건물이 있다면 오류
        if (account.BuildingInfos.Any(b => b.BuildingType == buildingType))
        {
            return BadRequest("Building already exists");
        }
        
        // 행복도가 충분하지 확인
        var hpi = value.HPI;
        if (hpi < 0 && account.Currency.HPI + hpi < 0)
        {
            return BadRequest("Not enough HPI");
        }

        var buildingInfo = new BuildingInfo
        {
            BuildingType = buildingType,
            OwnCount = 0,
            MaxCount = 1, // TODO : 건물 초기 최대 보유 개수 설정
        };

        account.BuildingInfos.Add(buildingInfo);
        await _context.SaveChangesAsync();

        return Ok(BuildingMapper.ToBuildingInfoDTO(buildingInfo));
    }
    
    /// <summary>
    /// 건물 최대 보유 개수를 증가시킵니다.
    /// 만약 최대 개수에 도달했거나 행복도가 충분하지 않으면 오류를 반환합니다.
    /// </summary>
    /// <param name="buildingType"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost("upgrademax")]
    public async Task<ActionResult<BuildingInfoUpgradResultDTO>> UpgradeBuildingInfoMaxCount(string buildingType)
    {
        var account = await GetAuthorizedAccountAsync();
        if (account == null) return Unauthorized("Account not found");
        
        var buildingInfo = account.BuildingInfos.FirstOrDefault(b => b.BuildingType == buildingType);
        if (buildingInfo == null)
        {
            return BadRequest("Building not found");
        }
        
        // 최대 보유 개수를 늘릴 수 있는지 확인
        if (buildingInfo.MaxCount >= _tableDataService.MaxBuildingCount)
        {
            return BadRequest("Max building count reached");
        }
        
        // 행복도가 충분하지 확인
        var hpi = _tableDataService.Building[buildingType].HPI;
        if (hpi < 0 && account.Currency.HPI + hpi < 0)
        {
            return BadRequest("Not enough HPI");
        }
        account.Currency.HPI += hpi;

        // 건물 최대 보유 개수 증가
        buildingInfo.MaxCount++;
        await _context.SaveChangesAsync();

        var buildingInfoResultDto = new BuildingInfoUpgradResultDTO()
        {
            BuildingInfo = BuildingMapper.ToBuildingInfoDTO(buildingInfo),
            UpdatedCurrency = CurrencyMapper.ToCurrencyDTO(account.Currency),
        };
        
        return Ok(buildingInfoResultDto);
    }
}