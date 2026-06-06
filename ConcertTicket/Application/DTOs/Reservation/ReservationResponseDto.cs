namespace ConcertTicket.Application.DTOs.Reservation
{
    public class ReservationResponseDto
    {
        public Guid ReservationId {  get; set; }
        public string Status { get; set; }
        public DateTime ExpiresAt { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
