namespace ConcertTicket.Application.DTOs.Reservation
{
    public class MyReservationDto
    {
        public Guid ReservationId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }

        //Una reserva puede tener 1 o mas boletos
        public List<MyTicketDto> Tickets { get; set; }
    }
}
