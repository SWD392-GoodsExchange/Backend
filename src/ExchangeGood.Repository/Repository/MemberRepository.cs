using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ExchangeGood.Contract.Common;
using ExchangeGood.Contract.DTOs;
using ExchangeGood.Contract.Enum.Member;
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
        var result = await _uow.MemberDAO.GetMemberById(feId,m => m.Role);
        if (result == null)
        {
            return default;
        }
    
        return result;
    }

    public async Task<Member> CheckLogin(LoginRequest loginRequest)
    {
        var result = await _uow.MemberDAO.GetMemberById(loginRequest.FeId, m => m.Role);
        if (result == null)
        {
            throw new MemberNotFoundException(loginRequest.FeId);
        }

        // check password
        using var hmac = new HMACSHA512(result.PasswordSalt);
        var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginRequest.Password));
        for (int i = 0; i < computeHash.Length; i++)
        {
            if (computeHash[i] != result.PasswordHash[i]) throw new PasswordInvalidException();
        }

        return result;
    }

    public async Task<bool> UpdatePassword(PasswordRequest passwordRequest)
    {
        var member = await _uow.MemberDAO.GetMemberById(passwordRequest.FeId);
        // check old password
        using var hmac = new HMACSHA512(member.PasswordSalt);
        var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(passwordRequest.OldPassword));
        for (int i = 0; i < computeHash.Length; i++)
        {
            if (computeHash[i] != member.PasswordHash[i]) throw new OldPasswordInvalidException();
        }
        // update new password
        using (var hash = new HMACSHA512())
        {
            member.PasswordHash = hash.ComputeHash(Encoding.UTF8.GetBytes(passwordRequest.NewPassword));
            member.PasswordSalt = hash.Key;
        }
        _uow.MemberDAO.Update(member);
        return await _uow.SaveChangesAsync();
    }

    public async Task<Member> CreateMember(CreateMemberRequest createMemberRequest)
    {
        var existMember = await _uow.MemberDAO.GetMemberById(createMemberRequest.FeId);
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
            .Dob(createMemberRequest.Dob)
            .Create();
        _uow.MemberDAO.Add(member);
        await _uow.SaveChangesAsync();
        var returnMember = await _uow.MemberDAO.GetMemberById(createMemberRequest.FeId,m => m.Role);
        return returnMember;
    }

    public async Task<Member> GetMemberByEmail(string email)
    {
        return await _uow.MemberDAO.GetMemberByEmail(email);
    }

    public async Task<IEnumerable<Top3MemberDto>> GetTop3PostingProducts()
    {
        var members = await _uow.MemberDAO.GetTop3MembersPostingProducts();
        return _mapper.Map<IEnumerable<Top3MemberDto>>(members);
    }

    public async Task<IEnumerable<Top3MemberDto>> GetTop3PostingProductsTradeType()
    {
        var members = await _uow.MemberDAO.GetTop3MembersPostingProductsTradeType();
        return _mapper.Map<IEnumerable<Top3MemberDto>>(members);
    }

    public async Task<int> GetTotalAccountsAsync()
    {
        return await _uow.MemberDAO.GetTotalAccountsAsync();
    }

    public async Task UpdateMemberStatus(string feId)
    {
        var member = await _uow.MemberDAO.GetMemberById(feId);
        if (member == null)
        {
            throw new MemberNotFoundException(feId);
        }

        member.Status = Status.Banned.Name;
        _uow.MemberDAO.Update(member);

        await _uow.SaveChangesAsync();
    }
}