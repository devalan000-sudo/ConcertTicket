using ConcertTicket.Application.DTOs.Venue;

namespace ConcertTicket.Application.Interfaces
{
    public interface IVenueService
    {
        Task<IEnumerable<VenueResponseDto>> GetAllAsync();
        Task<VenueResponseDto?> GetByIdAsync(Guid id);
        Task<VenueResponseDto> CreateAsync(CreateVenueDto request);
    }
}
