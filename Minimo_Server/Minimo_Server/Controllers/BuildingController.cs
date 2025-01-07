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
public class BuildingController(GameDbContext context, TimeService timeService) : ControllerBase
{
    private readonly GameDbContext _context = context;
    private readonly TimeService _timeService = timeService;

    private int? GetAccountIdFromClaims()
    {
        var accountClaim = User.Claims.FirstOrDefault(c => c.Type == "AccountId");
        return accountClaim != null && int.TryParse(accountClaim.Value, out var accountId) 
            ? accountId 
            : null;
    }

    private async Task<Account?> GetAccountAsync(int accountId) =>
        await _context.Accounts.Include(a => a.Buildings).FirstOrDefaultAsync(a => a.Id == accountId);

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
        var accountId = GetAccountIdFromClaims();
        if (accountId == null) return Unauthorized("AccountId claim not found or invalid");

        var account = await GetAccountAsync(accountId.Value);
        if (account == null) return NotFound(new { message = "Account not found" });

        var building = account.Buildings.FirstOrDefault(b => b.Id == id);
        return building == null
            ? NotFound(new { message = "Building not found" })
            : Ok(BuildingMapper.ToBuildingDTO(building));
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<BuildingDTO>> CreateBuilding(BuildingDTO buildingDto)
    {
        var accountId = GetAccountIdFromClaims();
        if (accountId == null) return Unauthorized("AccountId claim not found or invalid");

        var account = await GetAccountAsync(accountId.Value);
        if (account == null) return NotFound(new { message = "Account not found" });

        var building = new Building
        {
            Name = buildingDto.Name,
            Level = 1,
            CreatedAt = _timeService.CurrentTime,
            PositionVector = Vector3.Zero,
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
        var accountId = GetAccountIdFromClaims();
        if (accountId == null) return Unauthorized("AccountId claim not found or invalid");

        var account = await GetAccountAsync(accountId.Value);
        if (account == null) return NotFound(new { message = "Account not found" });

        var building = account.Buildings.FirstOrDefault(b => b.Id == updateParameter.Id);
        if (building == null) return NotFound(new { message = "Building not found" });

        if (updateParameter.IsInstalled.HasValue) building.IsInstalled = updateParameter.IsInstalled.Value;
        if (updateParameter.Position.HasValue) building.PositionVector = updateParameter.Position.Value;

        await _context.SaveChangesAsync();

        return Ok(BuildingMapper.ToBuildingDTO(building));
    }
}