namespace Unibean.Repository.Paging;

public class PagedResultModel<T>
{
    public List<T> Result { get; set; }
    public int CurrentPage { get; set; }
    public int PageCount { get; set; }
    public int PageSize { get; set; }
    public int RowCount { get; set; }
    public int TotalCount { get; set; }
}
