namespace ExpenseControl.Application.Common;

/// <summary>
/// Representa um resultado paginado de uma consulta.
/// </summary>
/// <typeparam name="T">Tipo dos itens da lista.</typeparam>
public class PagedResult<T>
{
    /// <summary>
    /// Lista de itens da página atual.
    /// </summary>
    public IEnumerable<T> Items { get; }

    /// <summary>
    /// Número da página atual (1-based).
    /// </summary>
    public int PageNumber { get; }

    /// <summary>
    /// Quantidade de itens por página.
    /// </summary>
    public int PageSize { get; }

    /// <summary>
    /// Total de itens em todas as páginas.
    /// </summary>
    public int TotalCount { get; }

    /// <summary>
    /// Total de páginas.
    /// </summary>
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    /// <summary>
    /// Indica se existe página anterior.
    /// </summary>
    public bool HasPreviousPage => PageNumber > 1;

    /// <summary>
    /// Indica se existe próxima página.
    /// </summary>
    public bool HasNextPage => PageNumber < TotalPages;

    public PagedResult(IEnumerable<T> items, int pageNumber, int pageSize, int totalCount)
    {
        Items = items;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;
    }
}

/// <summary>
/// Parâmetros de paginação para queries.
/// </summary>
public record PaginationParams(int PageNumber = 1, int PageSize = 10)
{
    /// <summary>
    /// Número máximo de itens por página.
    /// </summary>
    public const int MaxPageSize = 50;

    /// <summary>
    /// Valida e ajusta os parâmetros de paginação.
    /// </summary>
    public static PaginationParams Normalize(int pageNumber, int pageSize)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > MaxPageSize) pageSize = MaxPageSize;

        return new PaginationParams(pageNumber, pageSize);
    }
}
