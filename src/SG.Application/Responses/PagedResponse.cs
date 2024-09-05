namespace SG.Application.Responses;

public class PagedList<T> : ResultGeneric<T>
{
    public PagedList(T items, int count, int pageNumber, int pageSize, object? error = null, string errorMessage = "")
    {
        Data = items;
        IsSuccess  = error == null;
        Message =  errorMessage;
        Error = error;
        MetaData = new MetaData
        {
            TotalCount = count,
            PageSize = pageSize,
            CurrentPage = pageNumber,
            TotalPages = (int)Math.Ceiling(count / (double)pageSize),
        };
     }
    public MetaData MetaData { get; set; }
}

public class MetaData
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
}