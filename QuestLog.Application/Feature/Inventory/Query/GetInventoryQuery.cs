using MediatR;
using QuestLog.Application.Common.Models;
using QuestLog.Application.Dto;

namespace QuestLog.Application.Feature.Inventory.Query;

public class GetInventoryQuery : IRequest<PagedList<InventoryDto>>
{
    public string? SearchTerm { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;}