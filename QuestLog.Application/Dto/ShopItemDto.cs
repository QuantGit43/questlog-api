namespace QuestLog.Application.Dto;

public class ShopItemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public string? AssetId { get; set; } // Тут буде фінальний код картинки
    public string Slot { get; set; }
    public bool IsRecommended { get; set; }
    public string? Class { get; set; } = "Any";
}