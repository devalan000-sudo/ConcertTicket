using ConcertTicket.Application.DTOs.Event;
using ConcertTicket.Application.DTOs.Pagination;

namespace ConcertTicket.Application.Interfaces
{
    public interface IEventService
    {
        Task<EventResposeDto> CreateAsync(CreateEventDto request);
        Task<PageResponse<EventResposeDto>> GetAllPagedAsync(EventQueryFilter filter);
    }
}
