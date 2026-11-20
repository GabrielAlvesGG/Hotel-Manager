namespace hotel_manager.Mapping
{
    public record ClientListItem(string Name, string Email);

    public record ClientListResponse(
        IReadOnlyList<ClientListItem> Items,
        int TotalCount,
        int Page,
        int PageSize);
}
