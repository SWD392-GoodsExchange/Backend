using AutoMapper;
using AutoMapper.QueryableExtensions;
using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.DTOs;
using ExchangeGood.Contract.Payloads.Request.Members;
using ExchangeGood.DAO;
using ExchangeGood.Data.Models;
using ExchangeGood.Repository.Builders;
using ExchangeGood.Repository.Exceptions;
using ExchangeGood.Repository.Interfaces;

namespace ExchangeGood.Repository.Repository;

public class MemberRepository : IMemberRepository
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public MemberRepository(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<PagedList<MemberDto>> GetMembers(GetMembersQuery getMembersQuery)
    {
        var query = _uow.MemberDAO.GetAllMembers(getMembersQuery.SearchTerm, getMembersQuery.SortColumn,
            getMembersQuery.SortOrder);
        var result = await PagedList<MemberDto>.CreateAsync(query.ProjectTo<MemberDto>(_mapper.ConfigurationProvider),
            getMembersQuery.PageNumber, getMembersQuery.PageSize);
        return result;
    }

    public async Task<Member> GetMemberById(string feId)
    {
        var result = await _uow.MemberDAO.GetMemberById(feId);
        if (result == null)
        {
            return default;
        }

        return result;
    }

    public async Task<string> CreateMember(CreateMemberRequest createMemberRequest)
    {
        var existMember = await this.GetMemberById(createMemberRequest.FeId);
        if (existMember != null)
        {
            throw new ExistMemberException(createMemberRequest.FeId);
        }

        // use member builder to create a member object
        Member member = MemberBuilder
            .Empty()
            .PassWord(createMemberRequest.Password)
            .UserName(createMemberRequest.UserName)
            .FeId(createMemberRequest.FeId)
            .Address(createMemberRequest.Address)
            .Phone(createMemberRequest.Phone)
            .Email(createMemberRequest.Email)
            .Gender(createMemberRequest.Gender)
            .CreatedTime(DateTime.UtcNow)
            .UpdatedTime(DateTime.UtcNow)
            .Create();
        _uow.MemberDAO.Add(member);
        await _uow.SaveChangesAsync();
        return member.FeId;
    }
}