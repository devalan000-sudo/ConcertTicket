namespace ConcertTicket.Application.DTOs.Event
{
    public class CreateZoneDto
    {
        //Example: VIP, General, Oro etc...
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int TotalCapacity { get; set; }
    }
}
