using System;

namespace SG.Infrastructure.Base.Pagination;

public class SearchParameters
{
    public string SearchTerm { get; set; } = default!;
    public List<string> SearchFields { get; set; } = new List<string>();
}