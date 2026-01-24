using QuestLog.Application.Common.Models;

namespace QuestLog.Application.Common.Extensions;

public static class QueryableExtensions
{
    public static Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize)
    {
        return PagedList<T>.Create(source, pageNumber, pageSize);
    }
}