using ConcertTicket.Domain.Enums;

namespace ConcertTicket.Application.DTOs.Event
{
    public class CreateEventDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string ImageUrl { get; set; }

        public EventCategory Category { get; set; }
        public Guid VenueId { get; set; }
        public List<CreateZoneDto> Zones { get; set; } = new();
    }
}
