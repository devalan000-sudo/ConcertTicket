using ConcertTicket.Application.DTOs.Reservation;
using ConcertTicket.Application.Interfaces;
using ConcertTicket.Domain.Entities;
using ConcertTicket.Domain.Enums;

namespace ConcertTicket.Application.Services
{
    public class ReservationService : IReservationService
    {

        private readonly IRepository<Reservation> _reservationRepository;
        private readonly IRepository<Zone> _zoneRepository;
        private readonly ICacheService _cacheService;


        public ReservationService(
            IRepository<Reservation> reservationRepository,
            IRepository<Zone> zoneRepository,
            ICacheService cacheService
            )
        {
            _reservationRepository = reservationRepository;
            _zoneRepository = zoneRepository;
            _cacheService = cacheService;
        }

        public async Task<ReservationResponseDto> CreateReservationAsync(CreateReservationDto request, Guid userId)
        {
            //Validar si la zona existe y obtener su precio
            var zone = await _zoneRepository.GetByIdAsync(request.ZoneId);
            if (zone == null) throw new Exception("La zona no existe.");

            string redisKey = $"zone:{request.ZoneId}:available";

            //Operacion Atomica en Redis: Restamos los boletos solicitados
            var remainingTickets = await _cacheService.DecrementAsync(redisKey, request.Quantity);

            //Validar Overbooking
            if(remainingTickets < 0)
            {
                //Si bajo de cero, significa que no habia suficientes
                //Rollback en cache
                await _cacheService.IncrementAsync(redisKey,request.Quantity);
                throw new Exception("Boletos agotados o cantidad no disponible para esta zona/compra");
            }

            //Si llegamos hasta aqui, Redis nos garantizo los boletos, Procedemos con la accion de compra 
            var reservationId = Guid.NewGuid();
            var expirtaionTime = DateTime.UtcNow.AddMinutes(5);

            var reservation = new Reservation
            {
                Id = reservationId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = expirtaionTime,
                Status = ReservationStatus.Pending,
                TotalAmount = zone.Price * request.Quantity,
                Tickets = new List<Ticket>()
            };

            //Generamos lo boletos individuales
            for(int i = 0; i < request.Quantity; i++)
            {
                reservation.Tickets.Add(new Ticket
                {
                    Id = Guid.NewGuid(),
                    ReservationId = reservationId,
                    ZoneId = request.ZoneId,
                    QRToken = Guid.NewGuid().ToString("N"),
                    IsScanned = false,
                });
            }

            //Guardar en SQL
            await _reservationRepository.AddAsync(reservation);

            await _cacheService.SetAsync($"reservation:{reservationId}:lock",
                request.ZoneId,
                TimeSpan.FromMinutes(5));

            return new ReservationResponseDto
            {
                ReservationId = reservation.Id,
                Status = reservation.Status.ToString(),
                ExpiresAt = reservation.ExpiresAt,
                TotalAmount = reservation.TotalAmount
            };

            //
        }
    }
}
