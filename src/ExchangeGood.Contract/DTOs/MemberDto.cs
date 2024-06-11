namespace ExchangeGood.Contract.DTOs;

public class MemberDto
{
    public string FeId { get; set; }
    
    public string UserName { get; set; }
    
    public string Address { get; set; }

    public string Gender { get; set; }

    public string Email { get; set; }

    public string Phone { get; set; }

    public string Status { get; set; }

    public DateTime? Dob { get; set; }
}