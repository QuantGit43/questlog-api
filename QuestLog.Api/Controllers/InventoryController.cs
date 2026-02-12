using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuestLog.Application.Feature.Inventory.Commands;
using QuestLog.Application.Feature.Inventory.Query;

namespace QuestLog.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class InventoryController : Controller
{
  private readonly ISender _sender;
  
  public InventoryController(ISender sender)
  {
      _sender = sender;
  }

  [HttpPost("use")]
  public async Task<IActionResult> UseItem([FromBody] UseItemCommand command)
  {
      var result = await _sender.Send(command);
      return Ok(result);
  }

  [HttpPost("equip")]
  public async Task<IActionResult> EquipItem([FromBody] EquipItemCommand command)
  {
      var result = await _sender.Send(command);
      return Ok(result);
  }
  
  [HttpDelete]
    public async Task<IActionResult> RemoveItem(Guid itemId)
    {
        await _sender.Send(new DeleteInventoryItemCommand { ItemId = itemId });
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetInventoryItems([FromQuery] GetInventoryQuery query)
    {
        var items = await _sender.Send(query);
        return Ok(items);
    }
}