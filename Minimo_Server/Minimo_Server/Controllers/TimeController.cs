using Microsoft.AspNetCore.Mvc;
using MinimoServer.Models;
using MinimoServer.Services;

namespace MinimoServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TimeController : ControllerBase
{
    private readonly TimeService _timeService;
    
    public TimeController(TimeService timeService)
    {
        _timeService = timeService;
    }
    
    /// <summary>
    /// 현재 시간을 반환합니다.
    /// </summary>
    /// <returns>DateTime</returns>
    [HttpGet]
    public IActionResult GetTime()
    {
        return Ok(_timeService.CurrentTime);
    }
    
    /// <summary>
    /// 주어진 시간으로 서버의 시간을 설정합니다.
    /// </summary>
    /// <param name="targetTime">DateTime</param>
    /// <returns></returns>
    [HttpPut]
    public ActionResult<DateTime> SetTime([FromBody] DateTime targetTime)
    {
        _timeService.SetTime(targetTime);
        return Ok(_timeService.CurrentTime);
    }
}