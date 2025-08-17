namespace RedPhoenix.Web.Controllers;

using Microsoft.AspNetCore.Mvc;

using Data.Service;
using Data.ViewModels;
using Filters;

[ApiController]
[Route("api/[controller]")]
public class TableStatusController(ITableCurrentStatusService tableCurrentStatusService,
    IGameResultService gameResultService, IBettingService bettingService)
    : ControllerBase
{
    private readonly ITableCurrentStatusService _tableCurrentStatusService
        = tableCurrentStatusService
        ?? throw new ArgumentNullException(nameof(tableCurrentStatusService));
    private readonly IGameResultService _gameResultService = gameResultService
        ?? throw new ArgumentNullException(nameof(gameResultService));
    private readonly IBettingService _bettingService = bettingService
        ?? throw new ArgumentNullException(nameof(bettingService));


    [HttpPost]
    [Route("TableCurrentStatusUpdate")]
    [ModelValidation]
    public async Task<IActionResult> TableCurrentStatusUpdate(TableCurrentStatusViewModel model)
    {
        
        var currentStatusCache = _tableCurrentStatusService.TableStatusUpdate(model);
        await _tableCurrentStatusService.TableStatusMessageSend(currentStatusCache);

        switch (model.CurrentGameStatus)
        {
            case "1":
                await _gameResultService.GameResultCreateMessageSend(currentStatusCache);
                break;
            case "5":
                //Todo 경기마감 배팅마감관련 Messae 처리
                await _gameResultService.GameResultFinishMessageSend(currentStatusCache);
                await _bettingService.BettingFinishedMessageSend(model);
                break;
        }

        return Ok();
    }

    [HttpGet]
    [Route("TableCurrentStatus")]
    public IActionResult TableCurrentStatus(string tableId)
    {
        var result =  _tableCurrentStatusService.GetTableCurrentStatus(tableId);
        return Ok(result);
    }

    [HttpGet]
    [Route("TableResultSummary")]
    public IActionResult TableResultSummary(string tableId)
    {
        var result = _tableCurrentStatusService.GetResultSummary(tableId);
        return Ok(result);
    }

    [HttpGet]
    [Route("TableStatusInit")]
    public async Task<IActionResult> TableStatusInit(string tableId)
    {
        _tableCurrentStatusService.TableStatusInit(tableId);
        var currentStatusCache = _tableCurrentStatusService
            .GetTableCurrentCacheInfo(tableId);

        await _tableCurrentStatusService.TableStatusMessageSend(currentStatusCache);

        return Ok();
    }

}

