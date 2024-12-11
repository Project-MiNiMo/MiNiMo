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
    
    [HttpGet]
    public IActionResult GetTime()
    {
        return Ok(_timeService.CurrentTime);
    }
    
    [HttpPut]
    public IActionResult SetTime([FromBody] DateTime targetTime)
    {
        _timeService.SetTime(targetTime);
        return Ok();
    }
}