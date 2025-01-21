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
        var updatedItems = new List<Item>();
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
                updatedItems.Add(item1);
            }
            if (item2Require > 0)
            {
                if(account.Items.FirstOrDefault(i => i.ItemType == item2Id) is not { } item2 || item2.Count < item2Require)
                {
                    return BadRequest("Not enough item2");
                }
                item2.Count -= item2Require;
                updatedItems.Add(item2);
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
            UpdatedItems = updatedItems.Select(ItemMapper.ToItemDTO).ToList(),
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
        
        // 아이템이 충분한지 확인 후 차감
        bool isItemEnough = true;
        var recipe = _tableDataService.Produce[building.Type]?.ProduceOptions[startProduceDto.RecipeId - 1]; // 우선 인덱스는 구글 드라이브 테이블을 따름
        if (recipe == null) return BadRequest("Recipe not found");
        if (recipe.Materials.Length > 0)
        {
            foreach (var material in recipe.Materials)
            {
                var accountItem = account.Items.FirstOrDefault(i => i.ItemType == material.Code);
                if (accountItem == null || accountItem.Count < material.Amount)
                {
                    isItemEnough = false;
                    break;
                }
                accountItem.Count -= material.Amount;
            }
        }
        if (!isItemEnough) return BadRequest("Not enough items");
        
        // 레시피 building에 저장
        building.Recipes[slotIndex] = startProduceDto.RecipeId;
        building.ProduceStatus[slotIndex] = false;

        // 생산 가능 여부 확인 후 첫 번째 슬롯 시작
        await TryStartNextProduction(building);

        await _context.SaveChangesAsync();

        return Ok(BuildingMapper.ToBuildingDTO(building));
    }
    
    private async Task TryStartNextProduction(Building building, int currentSlotIndex = -1)
    {
        // 이미 생산 중인 슬롯이 있는지 확인
        if (building.ProduceStatus.Any(status => status))
        {
            // 생산 중인 슬롯이 있으므로 새로운 생산을 시작하지 않음
            return;
        }
        
        var startIndex = (currentSlotIndex + 1) % building.ProduceStatus.Length;
        
        // 시작 인덱스부터 false 상태인 슬롯 찾기
        var nextSlotIndex = -1;
        for (var i = 0; i < building.ProduceStatus.Length; i++)
        {
            var index = (startIndex + i) % building.ProduceStatus.Length; // 순환 처리
            if (!building.ProduceStatus[index])
            {
                nextSlotIndex = index;
                break;
            }
        }

        // 생산할 슬롯이 없으면 종료
        if (nextSlotIndex == -1) return;

        // 슬롯의 레시피를 확인하고 생산 시작
        var recipeId = building.Recipes[nextSlotIndex];
        if (recipeId <= 0) return;

        var recipe = _tableDataService.GetProduceRecipe(building.Type, recipeId);
        if (recipe == null) return;

        building.ProduceStartAt[nextSlotIndex] = _timeService.CurrentTime;
        building.ProduceEndAt[nextSlotIndex] = building.ProduceStartAt[nextSlotIndex] + TimeSpan.FromSeconds(recipe.Time);
        building.ProduceStatus[nextSlotIndex] = true;

        await _context.SaveChangesAsync();
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
        
        // 생산된 자원 추가
        var recipe = _tableDataService.GetProduceRecipe(building.Type, building.Recipes[slotIndex]);
        var updatedItemDTOs = new List<ItemDTO>();
        if (recipe == null) return BadRequest("Recipe not found");
        foreach (var result in recipe.Results)
        {
            var accountItem = account.Items.FirstOrDefault(i => i.ItemType == result.Code);
            if (accountItem == null)
            {
                accountItem = new Item
                {
                    ItemType = result.Code,
                    Count = result.Amount,
                };
                account.Items.Add(accountItem);
            }
            else
            {
                accountItem.Count += result.Amount;
            }
            updatedItemDTOs.Add(ItemMapper.ToItemDTO(accountItem));
        }
        
        // 경험치 추가
        account.Experience += recipe.EXP;
        
        // 레시피 building에 저장
        building.Recipes[slotIndex] = 0;
        building.ProduceStatus[slotIndex] = false;
        
        await _context.SaveChangesAsync();
        
        await TryStartNextProduction(building, slotIndex);
        
        var resultDTO = new BuildingCompleteProduceResultDTO()
        {
            UpdatedBuilding = BuildingMapper.ToBuildingDTO(building),
            ProducedItems = updatedItemDTOs,
            UpdatedCurrency = CurrencyMapper.ToCurrencyDTO(account.Currency),
        };
        return Ok(resultDTO);
    }

    /// <summary>
    /// 현금을 지불하여 즉시 생산을 완료합니다.
    /// </summary>
    /// <param name="instantProduceDto"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost("recipe/instant")]
    public async Task<ActionResult<BuildingInstantProduceResultDTO>> InstantProduce(BuildingInstantProduceDTO instantProduceDto)
    {
        var account = await GetAuthorizedAccountAsync();
        if (account == null) return Unauthorized("Account not found");

        var building = account.Buildings.FirstOrDefault(b => b.Id == instantProduceDto.BuildingId);
        if (building == null) return NotFound(new { message = "Building not found" });

        var buildingInfo = account.BuildingInfos.FirstOrDefault(b => b.BuildingType == building.Type);
        if (buildingInfo == null) return BadRequest("BuildingInfo not found");

        var slotIndex = instantProduceDto.SlotIndex;
        if (slotIndex < 0 || slotIndex >= buildingInfo.ProduceSlotCount) return BadRequest("Invalid slot index");

        if (!building.ProduceStatus[slotIndex]) return BadRequest("Not producing");

        var remainingTime = building.ProduceEndAt[slotIndex] - _timeService.CurrentTime;
        if (remainingTime <= TimeSpan.Zero) return BadRequest("Production already completed");

        // 계산된 현금 소모량 확인 및 차감
        var cashCost = CalculateInstantProduceCost(remainingTime);
        if (account.Currency.BlueStar < cashCost) return BadRequest("Not enough blue star");
        account.Currency.BlueStar -= cashCost;

        // 생산 종료 시간 갱신
        building.ProduceEndAt[slotIndex] = _timeService.CurrentTime;
          
        await _context.SaveChangesAsync();
        
        //await TryStartNextProduction(building, slotIndex);
        
        return Ok(new BuildingInstantProduceResultDTO
        {
            UpdatedBuilding = BuildingMapper.ToBuildingDTO(building),
            UpdatedCurrency = CurrencyMapper.ToCurrencyDTO(account.Currency),
        });
    }

    private int CalculateInstantProduceCost(TimeSpan remainingTime)
    {
        return (int)Math.Ceiling(remainingTime.TotalSeconds / 30);
    }
}