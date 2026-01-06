namespace QuestLog.Application.Interfaces
{
    public interface IAiService
    {
        Task<string> GetAnswerAsync(string prompt);
    }
}