namespace QuestLog.Application.Exceptions;

public class CredentialsConflictException(string message) : Exception(message);