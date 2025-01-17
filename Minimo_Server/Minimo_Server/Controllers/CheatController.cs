using Microsoft.AspNetCore.Mvc;
using MinimoServer.Models;
using MinimoServer.Services;
using MinimoShared;

namespace MinimoServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CheatController(GameDbContext context, TimeService timeService, TableDataService tableDataService) : BaseController(context, timeService, tableDataService)
{
    /// <summary>
    /// 재화(Star, BlueStar, Heart, HPI)를 강제로 업데이트합니다.
    /// </summary>
    /// <param name="currencyDto"></param>
    /// <returns></returns>
    [HttpPost("currency")]
    public async Task<ActionResult<CurrencyDTO>> UpdateCurrency([FromBody] CurrencyDTO currencyDto)
    {
        var account = await GetAuthorizedAccountAsync();
        if (account == null) return Unauthorized("Account not found");

        account.Currency = CurrencyMapper.ToCurrency(currencyDto);
        await _context.SaveChangesAsync();
        
        // Send UpdateAssetDTO
        return Ok(CurrencyMapper.ToCurrencyDTO(account.Currency));
    }
    
    /// <summary>
    /// 아이템을 강제로 업데이트합니다.(없으면 추가합니다.)
    /// </summary>
    /// <param name="itemDto"></param>
    /// <returns></returns>
    [HttpPost("item")]
    public async Task<ActionResult<ItemDTO>> UpdateItem([FromBody] ItemDTO itemDto)
    {
        var account = await GetAuthorizedAccountAsync();
        if (account == null) return Unauthorized("Account not found");
        
        // 아이템이 유효한지 검사
        if(_tableDataService.Item.ContainsKey(itemDto.ItemType) == false)
        {
            return BadRequest("Invalid ItemType");
        }

        // 아이템이 이미 존재하는 경우, 개수만 업데이트
        var item = account.Items.Find(i => i.ItemType == itemDto.ItemType);
        if (item != null)
        {
            item.Count = itemDto.Count;
        }
        else
        {
            account.Items.Add(ItemMapper.ToItem(itemDto));
        }
        await _context.SaveChangesAsync();
        
        return Ok(ItemMapper.ToItemDTO(item));
    }
    
    /// <summary>
    /// 건물 정보를 강제로 업데이트합니다.(없으면 추가합니다.)
    /// </summary>
    /// <param name="buildingInfoDto"></param>
    /// <returns></returns>
    [HttpPost("buildinginfo")]
    public async Task<ActionResult<BuildingInfoDTO>> UpdateBuildingInfo([FromBody] BuildingInfoDTO buildingInfoDto)
    {
        var account = await GetAuthorizedAccountAsync();
        if (account == null) return Unauthorized("Account not found");
        
        // 유효 검사 1: 소유 개수가 최대 개수보다 많거나 최대 개수가 최대 건물 개수보다 많을 경우 오류
        if (buildingInfoDto.OwnCount > buildingInfoDto.MaxCount || buildingInfoDto.MaxCount > _tableDataService.MaxBuildingCount)
        {
            return BadRequest("Invalid BuildingInfoDTO. Check the ownCount and maxCount.");
        }
        
        // 유효 검사 2: ProduceSlotCount가 최대값보다 큰 경우 오류
        if (buildingInfoDto.ProduceSlotCount > _tableDataService.MaxProduceSlotCount)
        {
            return BadRequest("Invalid BuildingInfoDTO. Check the produceSlotCount.");
        }
        
        // find the buildingOwnInfo
        var buildingInfo = account.BuildingInfos.Find(b => b.BuildingType == buildingInfoDto.BuildingType);
        if (buildingInfo != null)
        {
            buildingInfo.OwnCount = buildingInfoDto.OwnCount;
            buildingInfo.MaxCount = buildingInfoDto.MaxCount;
            buildingInfo.ProduceSlotCount = buildingInfoDto.ProduceSlotCount;
        }
        else
        {
            buildingInfo = BuildingMapper.ToBuildingInfo(buildingInfoDto);
            buildingInfo.InstallCount = 0;
            account.BuildingInfos.Add(buildingInfo);
        }
        await _context.SaveChangesAsync();
        
        return Ok(BuildingMapper.ToBuildingInfoDTO(buildingInfo));
    }
}