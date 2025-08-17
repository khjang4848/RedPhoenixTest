namespace RedPhoenix.Web.Controllers;

using Microsoft.AspNetCore.Mvc;
using Data.Service;
using Data.Models;

[ApiController]
[Route("api/[controller]")]
public class TableController(ITableInfoService tableInfoService) : ControllerBase
{
    private readonly ITableInfoService _tableInfoService = tableInfoService 
        ?? throw new ArgumentNullException(nameof(tableInfoService));

    [HttpGet]
    [Route("TableInfo")]
    public async Task<IActionResult> GetTableInfo(string tableId)
    {
        var result = await _tableInfoService.GetTableInfo(tableId);
        return Ok(result);
    }

    [HttpGet]
    [Route("TableInfoAll")]
    public async Task<IActionResult> GetTableInfoAll()
    {
        var result = await _tableInfoService.GetTableInfoAll();
        return Ok(result);
    }

    [HttpPost]
    [Route("InsertTableInfo")]
    public async Task<IActionResult> InsertTableInfo(TableInfo info)
    {
        await _tableInfoService.InsertTableInfo(info);
        return Ok();
    }

    [HttpPost]
    [Route("UpdateTableInfo")]
    public async Task<IActionResult> UpdateTableInfo(TableInfo info)
    {
        await _tableInfoService.UpdateTableInfo(info);
        return Ok();
    }

}