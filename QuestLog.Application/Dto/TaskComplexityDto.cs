namespace QuestLog.Application.Dto;

public class TaskComplexityDto
{
    public string Difficulty { get; set; } 
    public string Category { get; set; }   
    public DateTime DueDate { get; set; }  
    
    public int XpReward { get; set; }
    public int GoldReward { get; set; }
}