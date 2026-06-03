using ConcertTicket.Domain.Enums;

namespace ConcertTicket.Domain.Entities
{
    public class Reservation
    {
        public Guid Id { get; set; }
        public DateTime CretedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public ReservationStatus Status  { get; set; }
        public decimal TotalAmount { get; set; }


        //Relaciones
        public Guid UserId { get; set; }
        public User User { get; set; }

        public ICollection<Ticket> Tickets { get; set; }
    }
}
