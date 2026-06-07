namespace ConcertTicket.Application.DTOs.Reservation
{
    public class MyTicketDto
    {
        public Guid TicketId { get; set; }
        public string EventTitle { get; set; }
        public DateTime EventDate { get; set; }
        public string VenueName { get; set; }
        public string ZoneName { get; set; }
        public string QRToken { get; set; }
        public bool IsScanned { get; set; }
    }
}
