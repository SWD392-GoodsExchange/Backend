using ExchangeGood.Contract.Common;
using ExchangeGood.Data.Models;

namespace ExchangeGood.Contract.Payloads.Request.Members;

public class GetMembersQuery : PaginationParams
{
    public string SearchTerm { get; set; }
    public string SortColumn { get; set; }
    public string SortOrder { get; set; }
    
}