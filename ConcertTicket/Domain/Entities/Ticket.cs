namespace ConcertTicket.Domain.Entities
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public string QRToken { get; set; }
        public bool IsScanned { get; set; }

        //Relaciones
        public Guid ReservationId { get; set; }
        public Reservation Reservation { get; set; }
        public Guid ZoneId { get; set; }
        public Zone Zone { get; set; }
    }
}
