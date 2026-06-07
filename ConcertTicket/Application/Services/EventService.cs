using System.Linq;
using ConcertTicket.Application.DTOs.Event;
using ConcertTicket.Application.DTOs.Pagination;
using ConcertTicket.Application.Interfaces;
using ConcertTicket.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConcertTicket.Application.Services
{
    public class EventService : IEventService
    {
        private readonly IRepository<Event> _eventRepository;
        private readonly IRepository<Venue> _venueRepository;
        private readonly ICacheService _cacheService;

        public EventService(IRepository<Event> eventRepository, IRepository<Venue> venueRepository, ICacheService cacheService)
        {
            _eventRepository = eventRepository;
            _venueRepository = venueRepository;
            _cacheService = cacheService;
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
                ImageUrl = request.ImageUrl,
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

            foreach (var zone in newEvent.Zones)
            {
                await _cacheService.SetAsync($"zone:{zone.Id}:available", zone.TotalCapacity, null);
            }

            return new EventResposeDto
            {
                Id = newEvent.Id,
                Title = newEvent.Title,
                Date = newEvent.Date,
                Category = newEvent.Category.ToString(),
                TotalZones = newEvent.Zones.Count,
            };
        }

        public async Task<PageResponse<EventResposeDto>> GetAllPagedAsync(EventQueryFilter filter)
        {
            var query = _eventRepository.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
                query = query.Where(e => e.Title.Contains(filter.SearchTerm));

            if (!string.IsNullOrWhiteSpace(filter.Category))
                query = query.Where(e => e.Category.ToString() == filter.Category);

            var totalRecords = await query.CountAsync();

            var events = await query
                .OrderBy(e => e.Date)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(e => new EventResposeDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Date = e.Date,
                    Category = e.Category.ToString(),
                    TotalZones = e.Zones != null ? e.Zones.Count : 0
                }).ToListAsync();

            return new PageResponse<EventResposeDto>
            {
                Data = events,
                TotalRecords = totalRecords,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }
    }
}
