using Microsoft.EntityFrameworkCore;

namespace QuestLog.Application.Common.Models;

public class PagedList<T>
{
    public List<T> Items { get; private set; }
    public int PageNumber { get; private set; }
    public int TotalPages { get; private set; }
    public long TotalCount { get; private set; }
    
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;

    public PagedList(List<T> items, long count, int pageNumber, int pageSize)
    {
        TotalCount = count;
        PageNumber = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        Items = items;
    }
    
    public static async Task<PagedList<T>> Create(IQueryable<T> source, int pageNumber, int pageSize)
    {
        var count = await source.CountAsync();
        var items = source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
        
        return new PagedList<T>(items, count, pageNumber, pageSize);
    }
}