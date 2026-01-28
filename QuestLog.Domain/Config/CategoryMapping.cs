using QuestLog.Domain.Entities;
using QuestLog.Domain.Enums;

namespace QuestLog.Domain.Config;

public class CategoryMapping
{
    public static int GetStatBonus(Avatar avatar, TaskCategory category)
    {
        return category switch
        {
            TaskCategory.Sport or
            TaskCategory.Career or
            TaskCategory.Discipline => avatar.Strength,

            TaskCategory.Education or
            TaskCategory.Reading or
            TaskCategory.Tech => avatar.Intellect,

            TaskCategory.Art or
            TaskCategory.Chores or
            TaskCategory.Hobbies => avatar.Dexterity,

            TaskCategory.Health or
            TaskCategory.Family or
            TaskCategory.SelfCare => avatar.Wisdom,

            _ => 0
        };
    }
}