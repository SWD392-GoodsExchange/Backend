namespace ExchangeGood.Contract.DTOs;

public class ExchangeRequestDto
{
    public int NotificationId { get; set; }
    public string SenderId { get; set; }
    public string SenderUsername { get; set; }
    public string RecipientId { get; set; }
    public string RecipientUsername { get; set; }
    public ProductDto OnwerProduct { get; set; }
    public IEnumerable<ProductDto> ExchangerProducts { get; set; }
    public string Content { get; set; }
    public DateTime? DateRead { get; set; }
    public DateTime CreatedDate { get; set; }
}