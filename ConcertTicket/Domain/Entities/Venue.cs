namespace ConcertTicket.Domain.Entities
{
    public class Venue
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Address { get; set; }

        public ICollection<Event> Events { get; set; }
    }
}
