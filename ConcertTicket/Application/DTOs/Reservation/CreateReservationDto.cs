namespace ConcertTicket.Application.DTOs.Reservation
{
    public class CreateReservationDto
    {
        public Guid ZoneId { get; set; }
        public int Quantity { get; set; }
    }
}
