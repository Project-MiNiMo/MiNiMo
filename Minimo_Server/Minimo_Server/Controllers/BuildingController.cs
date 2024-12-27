using System.Numerics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimoServer.Models;
using MinimoServer.Services;
using MinimoShared;
using Newtonsoft.Json.Linq;

namespace MinimoServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BuildingController(GameDbContext context, TimeService timeService) : ControllerBase
{
    /// <summary>
    /// 건물 목록을 반환합니다.
    /// </summary>
    /// <returns>List(BuildingDTO)</returns>
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BuildingDTO>>> GetBuildings()
    {
        var accountClaim = User.Claims.FirstOrDefault(c => c.Type == "AccountId");
        if (accountClaim == null)
        {
            return Unauthorized("AccountId claim not found");
        }
        
        var accountId = int.Parse(accountClaim.Value);
        
        var account = await context.Accounts
            .Include(a => a.Buildings)
            .FirstOrDefaultAsync(a => a.Id == accountId);
        
        if (account == null)
        {
            return NotFound(new { message = "Account not found" });
        }
        
        return Ok(account.Buildings.Select(BuildingMapper.ToBuildingDTO));
    }
    
    /// <summary>
    /// 주어진 id에 해당하는 건물을 반환합니다.
    /// </summary>
    /// <param name="id">건물의 id</param>
    /// <returns>BuildingDTO</returns>
    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<BuildingDTO>> GetBuilding(int id)
    {
        var accountClaim = User.Claims.FirstOrDefault(c => c.Type == "AccountId");
        if (accountClaim == null)
        {
            return Unauthorized("AccountId claim not found");
        }   
        
        var accountId = int.Parse(accountClaim.Value);
        
        var account = await context.Accounts
            .Include(a => a.Buildings)
            .FirstOrDefaultAsync(a => a.Id == accountId);
        if (account == null)
        {
            return NotFound(new { message = "Account not found" });
        }
        
        var building = account.Buildings.FirstOrDefault(b => b.Id == id);
        if (building == null)
        {
            return NotFound(new { message = "Building not found" });
        }
        
        return Ok(BuildingMapper.ToBuildingDTO(building));
    }
    
    /// <summary>
    /// 건물을 생성하고, 생성된 건물의 정보를 반환합니다.
    /// TODO: 자원이 부족할 경우 400 Bad Request를 반환합니다.
    /// </summary>
    /// <param name="buildingDto">BuildingDTO</param>
    /// <returns>BuildingDTO</returns>
    [HttpPost]
    public async Task<ActionResult<Building>> CreateBuilding(BuildingDTO buildingDto)
    {
        var accountIdClaim = User.Claims.FirstOrDefault(c => c.Type == "AccountId");
        if (accountIdClaim == null)
        {
            return Unauthorized("AccountId claim not found");
        }
        
        if (!int.TryParse(accountIdClaim.Value, out var accountId))
        {
            return Unauthorized("AccountId claim is not an integer");
        }
        
        var account = await context.Accounts
            .Include(a => a.Buildings)
            .FirstOrDefaultAsync(a => a.Id == accountId);
        
        if (account == null)
        {
            return NotFound(new { message = "Account not found" });
        }
        
        var building = new Building
        {
            Name = buildingDto.Name,
            Level = 1,
            CreatedAt = timeService.CurrentTime,
            PositionVector = Vector3.Zero,
            ProduceStartAt = DateTime.MinValue,
        };
        
        account.Buildings.Add(building);
        await context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetBuilding), new { accountId = accountId }, building);
    }

    /// <summary>
    /// 건물의 상태(설치여부, 위치)를 업데이트합니다.
    /// </summary>
    /// <param name="updateParameter">UpdateBuildingParameter</param>
    /// <returns>BuildingDTO(검증용)</returns>
    [HttpPut]
    public async Task<ActionResult<Building>> UpdateBuilding(UpdateBuildingParameter updateParameter)
    {
        var accountIdClaim = User.Claims.FirstOrDefault(c => c.Type == "AccountId");
        if (accountIdClaim == null)
        {
            return Unauthorized("AccountId claim not found");
        }
        
        if (!int.TryParse(accountIdClaim.Value, out var accountId))
        {
            return Unauthorized("AccountId claim is not an integer");
        }
        
        var account = await context.Accounts
            .Include(a => a.Buildings)
            .FirstOrDefaultAsync(a => a.Id == accountId);

        if (account == null)
        {
            return NotFound(new { message = "Account not found" });
        }
        
        var buildingId = updateParameter.Id;
        var building = account.Buildings.FirstOrDefault(b => b.Id == buildingId);
        if (building == null)
        {
            return NotFound(new { message = "Building not found" });
        }
        
        if (updateParameter.IsInstalled.HasValue)
        {
            building.IsInstalled = updateParameter.IsInstalled.Value;
        }
        
        if (updateParameter.Position.HasValue)
        {
            building.PositionVector = updateParameter.Position.Value;
        }
        
        await context.SaveChangesAsync();

        return Ok(BuildingMapper.ToBuildingDTO(building));
    }
}