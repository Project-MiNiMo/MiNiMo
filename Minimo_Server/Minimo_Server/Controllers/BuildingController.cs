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
public class BuildingController(GameDbContext context, TimeService timeService, TableDataService tableDataService) : BaseController(context, timeService, tableDataService)
{
    /// <summary>
    /// 건물 목록을 반환합니다.
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BuildingDTO>>> GetBuildings()
    {
        var accountId = GetAccountIdFromClaims();
        if (accountId == null) return Unauthorized("AccountId claim not found or invalid");

        var account = await GetAccountAsync(accountId.Value);
        return account == null
            ? NotFound(new { message = "Account not found" })
            : Ok(account.Buildings.Select(BuildingMapper.ToBuildingDTO));
    }

    /// <summary>
    /// 해당 id의 건물을 반환합니다.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<BuildingDTO>> GetBuilding(int id)
    {
        var account = await GetAuthorizedAccountAsync();
        if (account == null) return Unauthorized("Account not found");

        var building = account.Buildings.FirstOrDefault(b => b.Id == id);
        return building == null
            ? NotFound(new { message = "Building not found" })
            : Ok(BuildingMapper.ToBuildingDTO(building));
    }

    /// <summary>
    /// 새 건물을 생성합니다.
    /// 설치 개수가 최대 개수에 도달했거나 자원이 부족할 경우 오류를 반환합니다.
    /// </summary>
    /// <param name="buildingDto"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<BuildingCreateResultDTO>> CreateBuilding(BuildingDTO buildingDto)
    {
        var account = await GetAuthorizedAccountAsync();
        if (account == null) return Unauthorized("Account not found");
        
        // 설치 개수가 최대 개수에 도달했을 경우 오류
        var buildingInfo = account.BuildingInfos.FirstOrDefault(b => b.BuildingType == buildingDto.BuildingType);
        if (buildingInfo == null) return BadRequest("BuildingInfo not found");
        else if (buildingInfo.InstallCount >= buildingInfo.MaxCount) return BadRequest("Max building count reached");
        
        // 만약 설치 개수가 소유 개수에 도달했을 경우 자원이 충분한지 확인 후 차감
        if (buildingInfo.InstallCount >= buildingInfo.OwnCount)
        {
            var constructInfo = _tableDataService.Construct[buildingDto.BuildingType];
            var item1Id = constructInfo.MatCode1;
            var item1Require = constructInfo.MatAmount1;
            var item2Id = constructInfo.MatCode2;
            var item2Require = constructInfo.MatAmount2;

            if (item1Require > 0)
            {
                if(account.Items.FirstOrDefault(i => i.ItemType == item1Id) is not { } item1 || item1.Count < item1Require)
                {
                    return BadRequest("Not enough item1");
                }
                item1.Count -= item1Require;
            }
            if (item2Require > 0)
            {
                if(account.Items.FirstOrDefault(i => i.ItemType == item2Id) is not { } item2 || item2.Count < item2Require)
                {
                    return BadRequest("Not enough item2");
                }
                item2.Count -= item2Require;
            }
            buildingInfo.OwnCount++;
        }
        
        var building = new Building
        {
            Type = buildingDto.BuildingType,
            Level = 1,
            CreatedAt = _timeService.CurrentTime,
            Position = buildingDto.Position ?? new int[3],
        };
        account.Buildings.Add(building);
        buildingInfo.InstallCount++;
        await _context.SaveChangesAsync();

        var resultDto = new BuildingCreateResultDTO()
        {
            CreatedBuilding = BuildingMapper.ToBuildingDTO(building),
            BuildingInfoDto = BuildingMapper.ToBuildingInfoDTO(buildingInfo),
        };
        return Ok(resultDto);
    }

    /// <summary>
    /// 빌딩의 위치를 업데이트합니다.
    /// </summary>
    /// <param name="updateParameter"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    public async Task<ActionResult<BuildingDTO>> UpdateBuilding(UpdateBuildingParameter updateParameter)
    {
        var account = await GetAuthorizedAccountAsync();
        if (account == null) return Unauthorized("Account not found");

        var building = account.Buildings.FirstOrDefault(b => b.Id == updateParameter.Id);
        if (building == null) return NotFound(new { message = "Building not found" });

        if (updateParameter.Position != null) building.PositionVector = updateParameter.PositionVector;

        await _context.SaveChangesAsync();

        return Ok(BuildingMapper.ToBuildingDTO(building));
    }
    
    /// <summary>
    /// 빌딩을 제거합니다.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete]
    public async Task<ActionResult<BuildingDTO>> DeleteBuilding(int id)
    {
        var account = await GetAuthorizedAccountAsync();
        if (account == null) return Unauthorized("Account not found");

        var building = account.Buildings.FirstOrDefault(b => b.Id == id);
        if (building == null) return NotFound(new { message = "Building not found" });
        
        var buildingInfo = account.BuildingInfos.FirstOrDefault(b => b.BuildingType == building.Type);
        if (buildingInfo == null) return BadRequest("BuildingInfo not found");

        account.Buildings.Remove(building);
        buildingInfo.InstallCount--;
        await _context.SaveChangesAsync();

        return Ok(BuildingMapper.ToBuildingDTO(building));
    }
    
    /// <summary>
    /// 주어진 슬롯과 레시피로 제조를 시작합니다
    /// </summary>
    /// <param name="startProduceDto"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost("recipe")]
    public async Task<ActionResult<BuildingDTO>> StartProduce(BuildingStartProduceDTO startProduceDto)
    {
        var account = await GetAuthorizedAccountAsync();
        if (account == null) return Unauthorized("Account not found");

        var building = account.Buildings.FirstOrDefault(b => b.Id == startProduceDto.BuildingId);
        if (building == null) return NotFound(new { message = "Building not found" });
        
        var buildingInfo = account.BuildingInfos.FirstOrDefault(b => b.BuildingType == building.Type);
        if (buildingInfo == null) return BadRequest("BuildingInfo not found");
        
        var slotIndex = startProduceDto.SlotIndex;
        if (slotIndex < 0 || slotIndex >= buildingInfo.ProduceSlotCount) return BadRequest("Invalid slot index");
        
        if (building.ProduceStatus[slotIndex]) return BadRequest("Already producing");
        
        // TODO : 자원 충분한 지 확인
        
        // TODO : 자원 차감
        
        // 레시피 building에 저장
        building.Recipes[slotIndex] = startProduceDto.RecipeId;
        building.ProduceStartAt[slotIndex] = _timeService.CurrentTime;
        building.ProduceEndAt[slotIndex] = _timeService.CurrentTime.AddSeconds(10); // TODO : 제조 시간 설정
        building.ProduceStatus[slotIndex] = true;
        
        await _context.SaveChangesAsync();

        return Ok(BuildingMapper.ToBuildingDTO(building));
    }
    
    /// <summary>
    /// 주어진 슬롯의 제조를 완료하고 자원을 획득합니다.
    /// </summary>
    /// <param name="completeProduceDto"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost("recipe/complete")]
    public async Task<ActionResult<BuildingCompleteProduceResultDTO>> CompleteProduce(BuildingCompleteProduceDTO completeProduceDto)
    {
        var account = await GetAuthorizedAccountAsync();
        if (account == null) return Unauthorized("Account not found");

        var building = account.Buildings.FirstOrDefault(b => b.Id == completeProduceDto.BuildingId);
        if (building == null) return NotFound(new { message = "Building not found" });
        
        var buildingInfo = account.BuildingInfos.FirstOrDefault(b => b.BuildingType == building.Type);
        if (buildingInfo == null) return BadRequest("BuildingInfo not found");
        
        var slotIndex = completeProduceDto.SlotIndex;
        if (slotIndex < 0 || slotIndex >= buildingInfo.ProduceSlotCount) return BadRequest("Invalid slot index");
        
        if (!building.ProduceStatus[slotIndex]) return BadRequest("Not producing");
        
        if (_timeService.CurrentTime < building.ProduceEndAt[slotIndex]) return BadRequest("Production not completed yet");
        
        // TODO 자원 추가
        
        // 레시피 building에 저장
        building.Recipes[slotIndex] = 0;
        building.ProduceStatus[slotIndex] = false;
        
        await _context.SaveChangesAsync();
        
        var resultDTO = new BuildingCompleteProduceResultDTO()
        {
            UpdatedBuilding = BuildingMapper.ToBuildingDTO(building),
        };

        return Ok(resultDTO);
    }
}