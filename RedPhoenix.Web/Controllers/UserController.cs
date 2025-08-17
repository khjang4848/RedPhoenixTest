namespace RedPhoenix.Web.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Data.ViewModels;
using Data.Service;
using Filters;
using Data.Models;

[ApiController]
[Route("api/[controller]")]
public class UserController(IConfiguration config, 
    IAmountTransactionService amountTransactionService, 
    IMemberService memberService) : ControllerBase
{
    private readonly IConfiguration _config = config
        ?? throw new ArgumentNullException(nameof(config));
    private readonly IAmountTransactionService _transactionService = amountTransactionService
        ?? throw new ArgumentException(nameof(amountTransactionService));
    private readonly IMemberService _memberService = memberService
        ?? throw new ArgumentNullException(nameof(memberService));

    [HttpGet]
    [Route("Login")]
    public IActionResult Login()
    {
        var userInfo = new UserViewModel
        {
            Code = "M1212112",
            Id = "khjang10",
            Name = "khjang10",
            RoleCode = "Admin"
        };

        var token = new JsonWebToken(_config);

        return Ok(token.GenerateJwtToken(userInfo));
    }

    [HttpGet]
    [Route("TableLogin")]
    public IActionResult TableLogin(string tableId)
    {
        var userInfo = new UserViewModel
        {
            Code = "M1212112",
            Id = tableId,
            Name = tableId,
            RoleCode = "Admin"
        };
        var token = new JsonWebToken(_config);
        return Ok(token.GenerateJwtToken(userInfo));
    }


    [HttpGet]
    [Route("/Test")]
    [Authorize]
    public IActionResult Test()
    {
        return Ok("임수민 만세");
    }

    [HttpGet]
    [Route("UserAmount")]
    public IActionResult UserAmount()
    {
        return Ok(_transactionService.GetUserAmount("khjang10"));

    }

    [HttpGet]
    [Route("GetUserInfo")]
    public async Task<IActionResult> GetUserInfo(string memberId)
    {
        var result = await _memberService.GetUserInfo(memberId);
        return Ok(result);
    }

    [HttpPost]
    [Route("InsertUserInfo")]
    public async Task<IActionResult> InsertUserInfo(Member member)
    {
        await _memberService.InsertUserInfo(member);
        return Ok();
    }

    [HttpPost]
    [Route("UpdateUserAmount")]
    public async Task<IActionResult> UpdateUserAmount(UserAmountViewModel userAmount)
    {
        await _memberService.UpdateUserAmount(userAmount);
        return Ok();
    }

    [HttpPost]
    [Route("UpdateUserSessionKey")]
    public async Task<IActionResult> UpdateUserSessionKey(
        UserSessionKeyViewModel userSession)
    {
        await _memberService.UpdateUserSessionKey(userSession);
        return Ok();
    }
}

