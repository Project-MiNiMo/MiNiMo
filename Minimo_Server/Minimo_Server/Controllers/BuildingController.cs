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
public class BuildingController(GameDbContext context, TimeService timeService) : BaseController
{
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

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<BuildingDTO>> CreateBuilding(BuildingDTO buildingDto)
    {
        var account = await GetAuthorizedAccountAsync();
        if (account == null) return Unauthorized("Account not found");

        var building = new Building
        {
            Type = buildingDto.BuildingType,
            Level = 1,
            CreatedAt = _timeService.CurrentTime,
            Position = buildingDto.Position ?? new int[3],
            ProduceStartAt = DateTime.MinValue,
        };

        account.Buildings.Add(building);
        await _context.SaveChangesAsync();

        return Ok(BuildingMapper.ToBuildingDTO(building));
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult<Building>> UpdateBuilding(UpdateBuildingParameter updateParameter)
    {
        var account = await GetAuthorizedAccountAsync();
        if (account == null) return Unauthorized("Account not found");

        var building = account.Buildings.FirstOrDefault(b => b.Id == updateParameter.Id);
        if (building == null) return NotFound(new { message = "Building not found" });

        if (updateParameter.Position != null) building.PositionVector = updateParameter.PositionVector;

        await _context.SaveChangesAsync();

        return Ok(BuildingMapper.ToBuildingDTO(building));
    }
}