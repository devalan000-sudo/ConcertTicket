using ConcertTicket.Application.DTOs.Auth;

namespace ConcertTicket.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);
        Task<AuthResponseDto> VerifyEmailAsync(string token);
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    }
}
