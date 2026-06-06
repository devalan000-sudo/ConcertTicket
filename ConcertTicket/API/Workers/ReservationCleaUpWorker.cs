using ConcertTicket.Application.Interfaces;
using ConcertTicket.Domain.Enums;
using ConcertTicket.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ConcertTicket.API.Workers
{
    public class ReservationCleaUpWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ReservationCleaUpWorker> _logger;

        public ReservationCleaUpWorker(IServiceProvider serviceProvider, ILogger<ReservationCleaUpWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Iniciando el Worker de limpieza de reservas...");

            while (!stoppingToken.IsCancellationRequested)
            {
                await CleanupExpiredReservationsAsync();

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private async Task CleanupExpiredReservationsAsync()
        {
            using var scope = _serviceProvider.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var cacheService = scope.ServiceProvider.GetRequiredService<ICacheService>();

            try
            {
                var expiredReservations = await dbContext.Reservations
                    .Include(r => r.Tickets)
                    .Where(r => r.Status == ReservationStatus.Pending && r.ExpiresAt <= DateTime.UtcNow)
                    .ToListAsync();

                if (!expiredReservations.Any()) return;
                _logger.LogInformation($"Se encontraron {expiredReservations.Count} reservas expiradas. Procesando.... ");

                foreach (var reservation in expiredReservations)
                {
                    reservation.Status = ReservationStatus.Expired;

                    var ticketsByZone = reservation.Tickets.GroupBy(t => t.ZoneId);

                    foreach (var group in ticketsByZone)
                    {
                        var zoneId = group.Key;
                        var quantityToReturn = group.Count();
                        var redisKey = $"zone:{zoneId}:available";

                        await cacheService.IncrementAsync(redisKey, quantityToReturn);
                        _logger.LogInformation($"Devueltos {quantityToReturn} boletos a la zona {zoneId}.");
                    }
                }

                await dbContext.SaveChangesAsync();
                _logger.LogInformation("Limpieza completada exitosamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrio un error limpiando las reservas expiradas.");
            }
        }
    }
}
