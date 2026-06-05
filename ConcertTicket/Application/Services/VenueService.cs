using ConcertTicket.Application.DTOs.Venue;
using ConcertTicket.Application.Interfaces;
using ConcertTicket.Domain.Entities;

namespace ConcertTicket.Application.Services
{
    public class VenueService : IVenueService
    {
        private readonly IRepository<Venue> _venueRepository;
        public VenueService(IRepository<Venue> venueRepository)
        {
            _venueRepository = venueRepository;
        }
        
        public async Task<VenueResponseDto> CreateAsync(CreateVenueDto request)
        {
            var newVenue = new Venue
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                City = request.City,
                Address = request.Address,
            };

            await _venueRepository.AddAsync(newVenue);

            return new VenueResponseDto
            {
                Id = newVenue.Id,
                Name = newVenue.Name,
                City = newVenue.City,
                Address = newVenue.Address,
            };
        }

        public async Task<IEnumerable<VenueResponseDto>> GetAllAsync()
        {
            var venues = await _venueRepository.GetAllAsync();

            //Mapeamos de Entidad a DTO para no exponer la BD
            return venues.Select(v => new VenueResponseDto
            {
                Id = v.Id,
                Name = v.Name,
                City = v.City,
                Address = v.Address,
            });
        }

        public async Task<VenueResponseDto?> GetByIdAsync(Guid id)
        {
            var venue = await _venueRepository.GetByIdAsync(id);
            if (venue == null) return null;

            return new VenueResponseDto
            {
                Id = venue.Id,
                Name = venue.Name,
                City = venue.City,
                Address = venue.Address,
            };
        }
    }
}
