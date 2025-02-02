﻿using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MinimoServer.Models;
using MinimoServer.Services;
using MinimoShared;

namespace MinimoServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController(JwtService jwtService, GameDbContext context, TimeService timeService)
    : ControllerBase
{
    /// <summary>
    /// 로그인을 시도하고, 성공 시 jwt 토큰과 현재시간, 계정정보를 반환합니다.
    /// </summary>
    /// <param name="loginDto">LoginDTO</param>
    /// <returns>{Token: string, Time: DateTime, Account: AccountDTO}</returns>
    [HttpPost]
    public async Task<IActionResult> Login(LoginDTO loginDto)
    {
        if (await GetValidUser(loginDto.Username, loginDto.Password) is { } account)
        {
            var token = jwtService.GenerateToken(account);
            var time = timeService.CurrentTime;
            return Ok(new { Token = token, Time = time, Account = AccountMapper.ToAccountDTO(account) });
        }

        return Unauthorized();
    }

    /// <summary>
    /// 토큰을 이용하여 로그인하고, 성공 시 현재시간과 계정정보를 반환합니다.
    /// </summary>
    /// <returns>{Time: DateTime, Account: AccountDTO}</returns>
    [Authorize]
    [HttpPost("token")]
    public async Task<IActionResult> LoginWithToken()
    {
        if (User.Claims.FirstOrDefault(c => c.Type == "AccountId") is { Value: var identity })
        {
            if (int.TryParse(identity, out var accountId) && await GetValidUser(accountId) is { } account)
            {
                var time = timeService.CurrentTime;
                return Ok(new { Time = time, Account = AccountMapper.ToAccountDTO(account) });
            }
        }
        
        return Unauthorized();
    }

    
    private async Task<Account?> GetValidUser(string username, string password)
    {
        // 데이터베이스에서 사용자를 찾고, 비밀번호가 일치하는지 확인
        var user = await context.Accounts
            .SingleOrDefaultAsync(u => u.Username == username && u.Password == password);

        return user;
    }
    
    private async Task<Account?> GetValidUser(string username)
    {
        // 데이터베이스에서 사용자를 찾음
        var user = await context.Accounts
            .SingleOrDefaultAsync(u => u.Username == username);

        return user;
    }
    
    private async Task<Account?> GetValidUser(int accountId)
    {
        // 데이터베이스에서 사용자를 찾음
        var user = await context.Accounts
            .SingleOrDefaultAsync(u => u.Id == accountId);

        return user;
    }
}