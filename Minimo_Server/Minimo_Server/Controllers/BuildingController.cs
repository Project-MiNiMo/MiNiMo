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
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BuildingDTO>>> GetBuildings(int accountId)
    {
        var buildings = await context.Buildings
            .Where(b => b.AccountId == accountId)
            .ToListAsync();
        return Ok(BuildingMapper.ToBuildingDTOs(buildings));
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<BuildingDTO>> GetBuilding(int id)
    {
        var building = await context.Buildings.FindAsync(id);
        
        if (building == null)
        {
            return NotFound(new { message = "Building not found" });
        }
        
        return Ok(BuildingMapper.ToBuildingDTO(building));
    }
    
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
        
        var building = new Building
        {
            Name = buildingDto.Name,
            Level = 1,
            CreatedAt = timeService.CurrentTime,
            PositionVector = buildingDto.Position,
            AccountId = accountId
        };
        
        context.Buildings.Add(building);
        await context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetBuilding), new { id = building.Id }, building);
    }

    [HttpPut]
    public async Task<ActionResult<Building>> UpdateBuilding(JObject updateParameter)
    {
        var buildingId = updateParameter["id"]?.Value<int>();
        if (buildingId == null)
        {
            return BadRequest(new { message = "Building ID not found" });
        }
        
        var building = await context.Buildings.FindAsync(buildingId);
        if (building == null)
        {
            return NotFound(new { message = "Building not found" });
        }
        
        var accountIdClaim = User.Claims.FirstOrDefault(c => c.Type == "AccountId");
        if (accountIdClaim == null)
        {
            return Unauthorized("AccountId claim not found");
        }
        
        if (!int.TryParse(accountIdClaim.Value, out var accountId))
        {
            return Unauthorized("AccountId claim is not an integer");
        }
        
        if (building.AccountId != accountId)
        {
            return Unauthorized("Building does not belong to the account");
        }
        
        var position = updateParameter["position"]?.Value<Vector3>();
        if (position != null)
        {
            building.PositionVector = position.Value;
        }
        
        await context.SaveChangesAsync();
        
        return Ok(building);
    }
    
    
    
}