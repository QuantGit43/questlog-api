namespace QuestLog.Application.Dto;

public class InventoryDto
{
    public Guid Id { get; set; }
    public Guid AvatarId { get; set; }
    public ItemDto Items { get; set; }
    public int Quantity { get; set; }
}