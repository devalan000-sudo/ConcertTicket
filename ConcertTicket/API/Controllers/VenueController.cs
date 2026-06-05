using ConcertTicket.Application.DTOs.Venue;
using ConcertTicket.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConcertTicket.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VenueController : ControllerBase
    {
        private readonly IVenueService _venueService;

        public VenueController(IVenueService venueService)
        {
            _venueService = venueService;
        }

        //Endpoint publico
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var venues = await _venueService.GetAllAsync();
            return Ok(venues);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var venue = await _venueService.GetByIdAsync(id);
            if(venue == null)
            {
                return NotFound(new { Message = "Recinto no encontrado." });
            }
            return Ok(venue);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateVenueDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var venue = await _venueService.CreateAsync(request);

            return CreatedAtAction(nameof(GetById), new { id = venue.Id }, venue);
        }
    }
}
