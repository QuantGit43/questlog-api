using MediatR;
using QuestLog.Application.Common.Models;
using QuestLog.Application.Dto;

namespace QuestLog.Application.Feature.Item.Query;

public class GetAllItemsQuery: IRequest<PagedList<ItemDto>>
{
    public string? SearchTerm { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}