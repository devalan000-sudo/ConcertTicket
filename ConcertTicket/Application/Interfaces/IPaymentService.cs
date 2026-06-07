using ConcertTicket.Application.DTOs.Payment;

namespace ConcertTicket.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentIntentResponseDto> CreatePaymentIntentAsync(Guid reservationId, Guid userId);
    }
}
