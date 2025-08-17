namespace RedPhoenix.Web.Controllers;

using Microsoft.AspNetCore.Mvc;

using Data.Service;
using Data.ViewModels;
using Data.Events;
using Filters;


[Route("api/[controller]")]
[ApiController]
public class BettingController(IBettingService bettingService,
    ITableCurrentStatusService tableCurrentStatus,
    IAmountTransactionService amountTransaction,
    IMemberService memberService) : ControllerBase
{
    private readonly IBettingService _bettingService = bettingService
        ?? throw new ArgumentNullException(nameof(bettingService));

    private readonly ITableCurrentStatusService _tableCurrentStatus
        = tableCurrentStatus
          ?? throw new ArgumentNullException(nameof(tableCurrentStatus));
    private readonly IAmountTransactionService _amountTransaction
        = amountTransaction
          ?? throw new ArgumentNullException(nameof(amountTransaction));

    private readonly IMemberService _memberService = memberService
            ?? throw new ArgumentNullException(nameof(memberService));


    [HttpPost]
    [Route("SingleBet")]
    [ModelValidation]
    public async Task<IActionResult> SingleBet(SingleBetViewModel model)
    {
        var userAmount = _memberService.GetUserAmount(model.MemberId);

        if (userAmount < model.Amount)
        {
            return BadRequest("사용자 보유머니가 적습니다");
        }

        var tableStatus = _tableCurrentStatus
            .GetTableCurrentStatus(model.GameId[..6]);

        if (tableStatus.Status != 2)
        {
            return BadRequest("현재 배팅중 상태가 아닙니다");
        }

        if (tableStatus.GameId != model.GameId)
        {
            return BadRequest("현재 진행중인 게임이 아닙니다");
        }

        if ((model.BettingCode is "P" or "B" or "T") && 
        (await _bettingService.GetImpossibleBettingCount(
            model.GameId, model.MemberId, model.BettingCode) > 0))
        {
            return BadRequest("다른 곳에 배팅이 되어 있습니다");
        }


        var bettingToken = await _bettingService.InsertBetting(model);
        var currentAmount = _amountTransaction.UpdateUserAmount(model.MemberId, 
            model.Amount * -1);
        var transaction = new TransactionCreated()
        {
            MemberId = model.MemberId,
            Amount = (model.Amount * -1),
            RefId = bettingToken,
            TransactionCode = 400,
            CurrentAmount = currentAmount
        };

        await _amountTransaction.SendTransactionCreated(transaction);
        await _bettingService.SingleBettingCreateMessageSend(model);
        
        return Ok(bettingToken);

    }

    [HttpPost]
    [Route("DoubleBet")]
    [ModelValidation]
    public async Task<IActionResult> DoubleBet(DoubleBetViewModel model)
    {
        var bettingAmount = model.BettingInfo.Sum(x => x.BettingAmount);

        var userAmount = _memberService.GetUserAmount(model.MemberId);

        if (userAmount < bettingAmount)
        {
            return BadRequest("사용자 보유머니가 적습니다");
        }

        var tableStatus = _tableCurrentStatus.GetTableCurrentStatus(model.GameId[..6]);

        if (tableStatus.Status != 2)
        {
            return BadRequest("현재 배팅중 상태가 아닙니다");
        }

        if (tableStatus.GameId != model.GameId)
        {
            return BadRequest("현재 진행중인 게임이 아닙니다");
        }

        var (token, amount) = await _bettingService.InsertBetting(model);
        var currentAmount = _amountTransaction.UpdateUserAmount(model.MemberId,
            amount * -1);

        var transaction = new TransactionCreated()
        {
            MemberId = model.MemberId,
            Amount = (amount * -1),
            RefId = token,
            TransactionCode = 400,
            CurrentAmount = currentAmount
        };

        await _amountTransaction.SendTransactionCreated(transaction);
        await _bettingService.DoubleBettingCreateMessageSend(model);
        
        return Ok(token);
    }

    [HttpPost]
    [Route("BettingCancel")]
    [ModelValidation]
    public async Task<IActionResult> BettingCancel(BettingCancelViewModel model)
    {
        
        var bettingCancelAmount = await _bettingService.GetBettingAmount(
            model.BettingCancelToken);

        if (bettingCancelAmount == 0)
        {
            return BadRequest("배팅중인 게임이 없습니다");
        }

        var tableStatus = _tableCurrentStatus
            .GetTableCurrentStatus(model.GameId[..6]);

        if (tableStatus.Status != 2)
        {
            return BadRequest("현재 배팅중 상태가 아닙니다");
        }

        if (tableStatus.GameId != model.GameId)
        {
            return BadRequest("현재 진행중인 게임이 아닙니다");
        }

        await _bettingService.CancelBetting(model.BettingCancelToken);

        var currentAmount = _amountTransaction.UpdateUserAmount(model.MemberId, 
            bettingCancelAmount);

        var transaction = new TransactionCreated()
        {
            MemberId = model.MemberId,
            Amount = bettingCancelAmount,
            RefId = model.BettingCancelToken,
            TransactionCode = 402,
            CurrentAmount = currentAmount
        };

        await _amountTransaction.SendTransactionCreated(transaction);
        await _bettingService.BettingCancelMessageSend(model);
        
        return Ok(currentAmount);
    }

}

