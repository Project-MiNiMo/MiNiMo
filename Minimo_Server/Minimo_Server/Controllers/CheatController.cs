using Microsoft.AspNetCore.Mvc;
using MinimoServer.Models;
using MinimoShared;

namespace MinimoServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CheatController : BaseController
{
    /// <summary>
    /// 재화(Star, BlueStar, Heart)를 강제로 업데이트한다.
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
    /// 아이템을 강제로 업데이트한다.(없으면 추가)
    /// </summary>
    /// <param name="itemDto"></param>
    /// <returns></returns>
    [HttpPost("item")]
    public async Task<ActionResult<ItemDTO>> UpdateItem([FromBody] ItemDTO itemDto)
    {
        var account = await GetAuthorizedAccountAsync();
        if (account == null) return Unauthorized("Account not found");

        // if item already exists, increase the count
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
    /// 건물 정보를 강제로 업데이트한다.(없으면 추가)
    /// </summary>
    /// <param name="buildingInfoDto"></param>
    /// <returns></returns>
    [HttpPost("building")]
    public async Task<ActionResult<BuildingInfoDTO>> UpdateBuildingInfo([FromBody] BuildingInfoDTO buildingInfoDto)
    {
        var account = await GetAuthorizedAccountAsync();
        if (account == null) return Unauthorized("Account not found");
        
        // check valid buildingInfoDto (매직넘버를 사용하지 않도록 별도의 테이블이 필요할듯)
        if (buildingInfoDto.OwnCount > buildingInfoDto.MaxCount || buildingInfoDto.MaxCount > _tableDataService.MaxBuildingCount)
        {
            return BadRequest("Invalid BuildingInfoDTO. Check the ownCount and maxCount.");
        }
        
        // find the buildingOwnInfo
        var buildingInfo = account.BuildingInfos.Find(b => b.BuildingType == buildingInfoDto.BuildingType);
        if (buildingInfo != null)
        {
            buildingInfo.OwnCount = buildingInfoDto.OwnCount;
            buildingInfo.MaxCount = buildingInfoDto.MaxCount;
        }
        else
        {
            buildingInfo = BuildingMapper.ToBuildingOwnInfo(buildingInfoDto);
            account.BuildingInfos.Add(buildingInfo);
        }
        await _context.SaveChangesAsync();
        
        return Ok(BuildingMapper.ToBuildingInfoDTO(buildingInfo));
    }
}