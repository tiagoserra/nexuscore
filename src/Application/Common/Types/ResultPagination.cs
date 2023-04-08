namespace Application.Common.Types;

public record ResultPagination(int TotalPages, int PageIndex, int PageSize, long Count, dynamic Itens)
{
    public bool HasPrevious => PageIndex > 0;

    public bool HasNext => PageIndex < TotalPages;
}
