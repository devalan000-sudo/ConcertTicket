using ConcertTicket.Domain.Enums;
using System.ComponentModel;

namespace ConcertTicket.Domain.Entities
{
    public class Event
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Descrption { get; set; }
        public DateTime Date {  get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }

        //Enums
        public EventCategory Category { get; set; }

        //Relaciones
        public Guid VenueId { get; set; }
        public Venue Venue { get; set; }
        public ICollection<Zone> Zones { get; set; }
    }
}
