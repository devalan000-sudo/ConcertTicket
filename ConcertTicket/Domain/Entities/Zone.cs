namespace ConcertTicket.Domain.Entities
{
    public class Zone
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int TotalCapacity { get; set; }
        public int AvailableTickets { get; set; }

        //Relaciones
        public Guid EventId { get; set; }
        public Event Event { get; set; }
        public ICollection<Ticket> Tickets { get; set; }

    }
}
