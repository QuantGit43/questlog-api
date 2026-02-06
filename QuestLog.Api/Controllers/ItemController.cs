using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuestLog.Application.Feature.Item.Commands;
using QuestLog.Application.Feature.Item.Query;
using QuestLog.Domain.Interfaces;

namespace QuestLog.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ItemController : Controller
{
    private readonly ISender _sender;
    
    public ItemController(ISender sender)
    {
        _sender = sender;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateItem([FromBody] CreateItemCommand command)
    {
        var id = await _sender.Send(command);
        return Ok(new { ItemId = id });
    }
    
    [HttpPost("buy")]
    public async Task<IActionResult> BuyItem([FromBody] BuyItemCommand command)
    {
        await _sender.Send(command);
        return Ok();
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteItem(Guid id)
    {
        await _sender.Send(new DeleteItemCommand { ItemId = id });
        return NoContent();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateItem([FromBody] UpdateItemCommand command)
    {
        await _sender.Send(command);
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetItemById(Guid id)
    {
        var query = new GetItemByIdQuery { Id = id };
        var result = await _sender.Send(query);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllItems()
    {
        var query = new GetAllItemsQuery();
        var result = await _sender.Send(query);
        return Ok(result);  
    }
}