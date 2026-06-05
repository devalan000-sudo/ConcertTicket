using ConcertTicket.Application.DTOs.Event;
using ConcertTicket.Application.Interfaces;
using ConcertTicket.Domain.Entities;

namespace ConcertTicket.Application.Services
{
    public class EventService : IEventService
    {
        private readonly IRepository<Event> _eventRepository;
        private readonly IRepository<Venue> _venueRepository;

        public EventService(IRepository<Event> eventRepository, IRepository<Venue> venueRepository)
        {
            _eventRepository = eventRepository;
            _venueRepository = venueRepository;
        }

        public async Task<EventResposeDto> CreateAsync(CreateEventDto request)
        {
            var venue = await _venueRepository.GetByIdAsync(request.VenueId);
            if (venue == null)
            {
                throw new Exception("El recinto especificado no existe...");
            }

            var eventId = Guid.NewGuid();

            var newEvent = new Event
            {
                Id = eventId,
                Title = request.Title,
                Description = request.Description,
                Date = request.Date,
                ImageUrl = request.imageUrl,
                Category = request.Category,
                VenueId = request.VenueId,
                IsActive = true,
                Zones = request.Zones.Select(z => new Zone
                {
                    Id = Guid.NewGuid(),
                    Name = z.Name,
                    Price = z.Price,
                    TotalCapacity = z.TotalCapacity,
                    AvailableTickets = z.TotalCapacity,
                    EventId = eventId,
                }).ToList(),
            };

            await _eventRepository.AddAsync(newEvent);

            return new EventResposeDto
            {
                Id = newEvent.Id,
                Title = newEvent.Title,
                Date = newEvent.Date,
                Category = newEvent.Category.ToString(),
                TotalZones = newEvent.Zones.Count,
            };
        }

        public async Task<IEnumerable<EventResposeDto>> GetAllAsync()
        {
            var events = await _eventRepository.GetAllAsync();

            return events.Select(e => new EventResposeDto
            {
                Id = e.Id,
                Title = e.Title,
                Date = e.Date,
                Category = e.Category.ToString(),
                TotalZones = e.Zones?.Count ?? 0
            }).ToList();
        }

        //
    }
}
