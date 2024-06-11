using AutoMapper;
using AutoMapper.QueryableExtensions;
using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.DTOs;
using ExchangeGood.Contract.Payloads.Request.Members;
using ExchangeGood.DAO;
using ExchangeGood.Data.Models;
using ExchangeGood.Repository.Interfaces;

namespace ExchangeGood.Repository.Repository;

public class MemberRepository : IMemberRepository
{
    private readonly MemberDAO _dao;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public MemberRepository(MemberDAO dao, IUnitOfWork uow, IMapper mapper)
    {
        _dao = dao;
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<PagedList<MemberDto>> GetMembers(GetMembersQuery getMembersQuery)
    {
        var query = _dao.GetAllMembers(getMembersQuery.SearchTerm, getMembersQuery.SortColumn,
            getMembersQuery.SortOrder);
        var result = await PagedList<MemberDto>.CreateAsync(query.ProjectTo<MemberDto>(_mapper.ConfigurationProvider),
            getMembersQuery.PageNumber,getMembersQuery.PageSize);
        return result;
    }

    public Task<Member> GetMemberById(int? Id)
    {
        throw new NotImplementedException();
    }
}