using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace ConcertTicket.Application.DTOs.Event
{
    public class EventResposeDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Category { get; set; }
        public int TotalZones { get; set; }
    }
}
