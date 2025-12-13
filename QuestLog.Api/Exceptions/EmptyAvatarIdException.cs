namespace QuestLog.Api.Exceptions;

public class EmptyAvatarIdException : Exception
{
    public EmptyAvatarIdException(string message) : base(message)
    {}
}