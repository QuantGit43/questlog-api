namespace QuestLog.Domain.Exeptions;

public class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException(string message) : base(message)
    {}
    
}