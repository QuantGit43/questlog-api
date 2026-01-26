namespace QuestLog.Application.Dto;

public record TaskComplexityDto
{
    public string Difficulty { get; set; }
    public int XpReward { get; set; }
    public int GoldReward { get; set; }
}