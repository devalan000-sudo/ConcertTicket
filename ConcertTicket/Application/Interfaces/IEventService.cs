using ConcertTicket.Application.DTOs.Event;

namespace ConcertTicket.Application.Interfaces
{
    public interface IEventService
    {
        Task<EventResposeDto> CreateAsync(CreateEventDto request);
        Task<IEnumerable<EventResposeDto>> GetAllAsync();
    }
}
