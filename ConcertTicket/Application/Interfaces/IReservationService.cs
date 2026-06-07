using ConcertTicket.Application.DTOs.Reservation;

namespace ConcertTicket.Application.Interfaces
{
    public interface IReservationService
    {
        Task<ReservationResponseDto> CreateReservationAsync(CreateReservationDto request, Guid userId);
        Task ConfirmPaymentAsync(Guid reservationId);

        Task<IEnumerable<MyReservationDto>> GetUserReservationsAsync(Guid userId);
    }
}
