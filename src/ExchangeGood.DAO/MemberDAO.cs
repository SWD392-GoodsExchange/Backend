using System.Linq.Expressions;
using ExchangeGood.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ExchangeGood.DAO;

public class MemberDAO
{
    private readonly GoodsExchangeContext _context;

    public MemberDAO(GoodsExchangeContext context)
    {
        _context = context;
    }

    public async Task<Member> GetMemberById(string feId, bool role = false)
    {
        var query = _context.Members.Where(x => x.FeId == feId);
        if (role)
        {
            query = query.Include(x => x.Role);
        }

        return await query.FirstOrDefaultAsync();
    }
    public IQueryable<Member> GetAllMembers(string searchTerm, string sortColumn, string sortOrder)
    {
        IQueryable<Member> membersQuery = _context.Members;
        if (!string.IsNullOrEmpty(searchTerm))
        {
            membersQuery = membersQuery.Where(p => p.UserName.ToLower().Contains(searchTerm.ToLower()));
            membersQuery = membersQuery.Where(p => p.Email.ToLower().Contains(searchTerm.ToLower()));
            membersQuery = membersQuery.Where(p => p.Gender.ToLower().Contains(searchTerm.ToLower()));
            membersQuery = membersQuery.Where(p => p.Phone.ToLower().Contains(searchTerm.ToLower()));
        }
        membersQuery = sortOrder?.ToLower() == "desc"
            ? membersQuery.OrderByDescending(GetSortExpression(sortColumn))
            : membersQuery.OrderBy(GetSortExpression(sortColumn));

        return membersQuery.AsNoTracking();
    }

    public Expression<Func<Member, object>> GetSortExpression(string sortColumn) =>
        // check if sort column is null => sort by feId, otherwise sort by option
        sortColumn?.ToLower() switch
        {
            "email" => member => member.Email,
            "name" => member => member.UserName,
            "gender" => member => member.Gender,
            "createdtime" => member => member.CreatedTime,
            "tpdatedtime" => member => member.UpdatedTime,
            _ => member => member.FeId
        };


    public void Update(Member member)
    {
        _context.Members.Update(member);
    }

    public void Add(Member member)
    {
        _context.Members.Add(member);
    }
    public void Delete(Member member)
    {
        _context.Members.Remove(member);
    }

    public async Task<Member> GetMemberByEmail(string email)
    {
        return await _context.Members.FirstOrDefaultAsync(m => m.Email == email);
    }


    public async Task<IEnumerable<Member>> GetTop3MembersPostingProducts()
    {
        var query = _context.Members
            .Select(member => new
            {
                Member = member,
                ProductCount = member.Products.Count()
            })
            .OrderByDescending(m => m.ProductCount)
            .Take(3)
            .Select(m => m.Member);

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Member>> GetTop3MembersPostingProductsTradeType()
    {
        var query = _context.Members
            .Select(member => new
            {
                Member = member,
                ProductCount = member.Products.Count(p => p.Type == "Trade")
            })
            .OrderByDescending(m => m.ProductCount)
            .Take(3)
            .Select(m => m.Member);

        return await query.ToListAsync();
    }

    public async Task<int> GetTotalAccountsAsync()
    {
        return await _context.Members.CountAsync();
    }

}