using ExchangeGood.Data.Models;

namespace ExchangeGood.Contract.Enum.Member;

public abstract class Role : Enumeration<Role>
{
    public static readonly Role Member = new MemberRole();
    public static readonly Role Admin = new AdminRole();
    public static readonly Role Moderator = new ModeratorRole();

    protected Role(int value, string name) : base(value, name)
    {
    }

    private sealed class MemberRole : Role
    {
        public MemberRole() : base(1, nameof(Member))
        {
        }
    }
    private sealed class AdminRole : Role
    {
        public AdminRole() : base(2, nameof(Admin))
        {
        }
    }
    private sealed class ModeratorRole:Role
    {
        public ModeratorRole() : base(3, nameof(Moderator))
        {
        }
    }
    
}