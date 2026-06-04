

namespace ConcertTicket.Application.DTOs.Auth
{
    public class LoginResponseDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public string FirstName { get; set; }
        public string Role {  get; set; }
    }
}
