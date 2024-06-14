namespace ExchangeGood.Contract.Payloads.Request.Members;

public class CreateMemberRequest
{
    public string FeId { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Address { get; set; }
    public string Gender { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public DateTime? Dob { get; set; }
}