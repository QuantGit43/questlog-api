namespace QuestLog.Domain.Enums;
using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TaskType
{
    Daily,
    Main,
    Habit
}