namespace ExchangeGood.Contract.DTOs;

public class NotificationDto
{
    public int NotificationId { get; set; }

    public string SenderId { get; set; }

    public string SenderUsername { get; set; }
    public string AvatarSender { get; set; }

    public string RecipientId { get; set; }

    public string RecipientUsername { get; set; }

    public string OnwerProductId { get; set; }

    public string ExchangerProductIds { get; set; }

    public string Content { get; set; }

    public DateTime? DateRead { get; set; }

    public DateTime CreatedDate { get; set; }

    public string Type { get; set; }
}