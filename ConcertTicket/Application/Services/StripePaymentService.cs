using ConcertTicket.Application.DTOs.Payment;
using ConcertTicket.Application.Interfaces;
using ConcertTicket.Domain.Entities;
using ConcertTicket.Domain.Enums;
using Stripe;

namespace ConcertTicket.Application.Services
{
    public class StripePaymentService : IPaymentService
    {
        private readonly IRepository<Reservation> _reservationRepository;
        private readonly IConfiguration _configuration;

        public StripePaymentService(IRepository<Reservation> reservationRepository, IConfiguration configuration)
        {
            _reservationRepository = reservationRepository;
            _configuration = configuration;
        }

        public async Task<PaymentIntentResponseDto> CreatePaymentIntentAsync(Guid reservationId, Guid userId)
        {
            var reservation = await _reservationRepository.GetByIdAsync(reservationId);

            if (reservation == null || reservation.UserId != userId)
                throw new Exception("Reserva no encontrada o acceso denegado");

            if (reservation.Status != ReservationStatus.Pending)
                throw new Exception("Esta reserva ya no esta disponible para pago (Expirada o ya pagada)");

            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(reservation.TotalAmount * 100),
                Currency = "mxn",
                Metadata = new Dictionary<string, string>
                {
                    {"ReservationId", reservation.Id.ToString() }
                }
            };

            var service = new PaymentIntentService();
            PaymentIntent intent = await service.CreateAsync(options);

            return new PaymentIntentResponseDto
            {
                ClientSecret = intent.ClientSecret,
                PublishableKey = _configuration["Stripe:PublishableKey"]
            };
        }
    }
}
