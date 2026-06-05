using ConcertTicket.Domain.Enums;

namespace ConcertTicket.Application.DTOs.Event
{
    public class CreateEventDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date {  get; set; }
        public string imageUrl { get; set; }

        //El Enum
        public EventCategory Category { get; set; }
        //El ID del recinto que ya creamos
        public Guid VenueId { get; set; }
        //Lista de zonas a crear
        public List<CreateZoneDto> Zones { get; set; } 
    }
}
