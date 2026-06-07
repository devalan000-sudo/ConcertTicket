namespace ConcertTicket.Application.DTOs.Pagination
{
    public class EventQueryFilter
    {
        public string? SearchTerm { get; set; } // Ej: Rock, Festival
        public string? Category { get; set; } // Ej: Concert, Sports

        //Valores por defecto de la paginacion
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
