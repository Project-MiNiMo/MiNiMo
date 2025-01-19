using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinimoServer.Models;
using MinimoServer.Services;
using MinimoShared;
using MinimoServer.Utility;

namespace MinimoServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MeteorController(GameDbContext context, TimeService timeService, TableDataService tableDataService)
    : BaseController(context, timeService, tableDataService)
{
    /// <summary>
    /// 유성 정보를 반환합니다.
    /// </summary>
    /// <param name="meteorType"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<MeteorDTO>>> GetMeteors()
    {
        var account = await GetAuthorizedAccountAsync();
        if (account == null) return Unauthorized("Account not found");

        return Ok(account.Meteors.Select(MeteorMapper.ToMeteorDTO).ToList());
    }

    /// <summary>
    /// 시간이 충분히 지난 경우, 유성을 생성합니다.
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet("create")]
    public async Task<ActionResult<List<MeteorDTO>>> CreateMeteors()
    {
        var account = await GetAuthorizedAccountAsync();
        if (account == null) return Unauthorized("Account not found");

        // 개수 도달했는지 확인
        var maxMeteorCount = _tableDataService.Common["MeteorLimit"];
        if (account.Meteors.Count >= maxMeteorCount)
        {
            return BadRequest("Meteor limit reached");
        }
        
        // 시간 비교
        var lastMeteorCreatedAt = account.LastMeteorCreatedAt;
        var spawnInterval = TimeSpan.FromSeconds(_tableDataService.Common["MeteorSpawnTime"]);
        var currentTime = _timeService.CurrentTime;
        if (lastMeteorCreatedAt + spawnInterval > currentTime)
        {
            return BadRequest("Not enough time has passed");
        }
        
        // 유성 생성
        var createdMeteors = new List<Meteor>();
        while (lastMeteorCreatedAt + spawnInterval <= currentTime && account.Meteors.Count < maxMeteorCount)
        {
            var meteor = CreateRandomMeteor(account.Level);
            account.Meteors.Add(meteor);
            createdMeteors.Add(meteor);
            lastMeteorCreatedAt += spawnInterval;
        }
        
        account.LastMeteorCreatedAt = lastMeteorCreatedAt;
        await _context.SaveChangesAsync();
        
        return Ok(createdMeteors.Select(MeteorMapper.ToMeteorDTO).ToList());
    }

    /// <summary>
    /// 유성 결과를 반환합니다.
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPut("Result")]
    public async Task<ActionResult<MeteorResultDTO>> GetMeteorResult(int meteorID)
    {
        var account = await GetAuthorizedAccountAsync();
        if (account == null) return Unauthorized("Account not found");

        var meteor = account.Meteors.FirstOrDefault(m => m.Id == meteorID);
        if (meteor == null) return BadRequest("Meteor not found");
        
        var isMaxMeteorCount = account.Meteors.Count >= _tableDataService.Common["MeteorLimit"];
        
        var isQuest = meteor.MeteorType is 0 or 1;
        if (isQuest)
        {
            var quest = new Quest
            {
                QuestType = meteor.ValueIndex,
            };
            account.Quests.Add(quest);
            account.Meteors.Remove(meteor);
            if(isMaxMeteorCount) account.LastMeteorCreatedAt = _timeService.CurrentTime;
            await _context.SaveChangesAsync();
            
            return Ok(new MeteorResultDTO
            {
                RemovedMeteorId = meteorID,
                ResultQuest = QuestMapper.ToQuestDTO(quest)
            });
        }
        else
        {
            var enumerator = _tableDataService.Item.Values.GetEnumerator();
            for (var i = 0; i < meteor.ValueIndex; i++)
            {
                enumerator.MoveNext();
            }

            var itemCode = enumerator.Current.ID;
            var accountItem = account.Items.FirstOrDefault(i => i.ItemType == itemCode);
            if (accountItem == null)
            {
                accountItem = new Item
                {
                    ItemType = itemCode,
                    Count = 0
                };
                account.Items.Add(accountItem);
            }
            accountItem.Count += meteor.ValueCount;
            account.Meteors.Remove(meteor);
            if(isMaxMeteorCount) account.LastMeteorCreatedAt = _timeService.CurrentTime;
            await _context.SaveChangesAsync();
            
            return Ok(new MeteorResultDTO
            {
                RemovedMeteorId = meteorID,
                ResultItem = ItemMapper.ToItemDTO(accountItem)
            });
        }
    }
    
    private Meteor CreateRandomMeteor(int level)
    {
        var meteorTypeProb = _tableDataService.Common["MeteorTypeProb"];
        var meteorNormalItemProb = _tableDataService.Common["Nomal_ItemTypeProb"];
        var meteorEpicItemProb = _tableDataService.Common["Epic_ItemTypeProb"];
        var meteorNormalQuestProb = _tableDataService.Common["Nomal_QuestTypeProb"];
        var meteorEpicQuestProb = _tableDataService.Common["Epic_QuestTypeProb"];

        var isQuest = new Random().Next(0, 100) < meteorTypeProb;
        if (isQuest)
        {
            var isEpic = new Random().Next(0, 100) < meteorEpicQuestProb;
            var meteorType = isEpic ? 1 : 0; // 기획서에서만 정의 되어 있음
            return new Meteor
            {
                MeteorType = meteorType,
                ValueIndex = _tableDataService.GetRandomQuestIndex(),
                ValueCount = 1,
            };
        }
        else
        {
            var isEpic = new Random().Next(0, 100) < meteorEpicItemProb;
            var meteorType = isEpic ? 4 : 3;
            return new Meteor
            {
                MeteorType = meteorType,
                ValueIndex = _tableDataService.GetRandomItemIndex(),
                ValueCount = _tableDataService.GetRandomItemCount()
            };
        }
    }
}