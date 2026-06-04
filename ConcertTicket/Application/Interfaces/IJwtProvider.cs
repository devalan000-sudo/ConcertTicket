using ConcertTicket.Domain.Entities;

namespace ConcertTicket.Application.Interfaces
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
    }
}
