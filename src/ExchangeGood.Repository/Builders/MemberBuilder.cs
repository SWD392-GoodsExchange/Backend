using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using ExchangeGood.Data.Models;
using Role = ExchangeGood.Contract.Enum.Member.Role;

namespace ExchangeGood.Repository.Builders;

public class MemberBuilder
{
    private string _feId;
    private int? _roleId;
    private string _userName;
    private byte[] _passwordHash;
    private byte[] _passwordSalt;
    private string _address;
    private string _gender;
    private string _email;
    private string _phone;
    private DateTime _createdTime;
    private DateTime _updatedTime;
    private string? _status;
    private DateTime? _dob;

    private MemberBuilder()
    {
    }

    public static MemberBuilder Empty() => new();

    public MemberBuilder FeId(string feId)
    {
        _feId = feId;
        return this;
    }
    public MemberBuilder RoleId(int roleId)
    {
        _roleId = Role.FromValue(roleId).Value;
        return this;
    }

    public MemberBuilder UserName(string userName)
    {
        _userName = userName;
        return this;
    }

    public MemberBuilder  PassWord(string password)
    {
        using (var hmac = new HMACSHA512())
        {
            _passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            _passwordSalt = hmac.Key;
        };
        return this;
    }

    public MemberBuilder Address(string address)
    {
        _address = address;
        return this;
    }

    public MemberBuilder Gender(string gender)
    {
        _gender = gender;
        return this;
    }

    public MemberBuilder Email(string email)
    {
        _email = email;
        return this;
    }

    public MemberBuilder CreatedTime(DateTime createdTime)
    {
        _createdTime = createdTime;
        return this;
    }

    public MemberBuilder UpdatedTime(DateTime updatedTime)
    {
        _updatedTime = updatedTime;
        return this;
    }

    public MemberBuilder Phone(string phone)
    {
        _phone = phone;
        return this;
    }

    public MemberBuilder Status(string? status)
    {
        _status = status;
        return this;
    }
    public MemberBuilder Dob(DateTime? dob)
    {
        _dob = dob;
        return this;
    }

    public Member Create()
    {
        return new Member()
        {
            FeId = _feId,
            RoleId = _roleId ?? Role.Member.Value,
            Phone = _phone,
            PasswordHash = _passwordHash,
            PasswordSalt = _passwordSalt,
            Address = _address,
            CreatedTime = _createdTime,
            UpdatedTime = _updatedTime,
            Status = _status ?? Contract.Enum.Member.Status.Available.Name,
            UserName = _userName,
            Gender = _gender,
            Email = _email,
            Dob = _dob
        };
    }
}