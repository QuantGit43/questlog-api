namespace QuestLog.Application.Exeptions;

public class ValidationException : Exception
{
    public ValidationException(string message) : base(message)
    {
    }
}