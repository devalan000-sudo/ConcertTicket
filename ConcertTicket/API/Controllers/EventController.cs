using ConcertTicket.Application.DTOs.Event;
using ConcertTicket.Application.DTOs.Pagination;
using ConcertTicket.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConcertTicket.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateEventDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await _eventService.CreateAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] EventQueryFilter filter)
        {
            var pagedResult = await _eventService.GetAllPagedAsync(filter);
            return Ok(pagedResult);
        }
    }
}
