namespace ConcertTicket.Application.DTOs.Payment
{
    public class PaymentIntentResponseDto
    {
        public string ClientSecret { get; set; }
        public string PublishableKey { get; set; }
    }
}
