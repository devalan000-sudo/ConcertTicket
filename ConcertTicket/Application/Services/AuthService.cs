using ConcertTicket.Application.DTOs.Auth;
using ConcertTicket.Application.Interfaces;
using ConcertTicket.Domain.Entities;
using ConcertTicket.Domain.Enums;

namespace ConcertTicket.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IJwtProvider _jwtProvider;

        public AuthService(IRepository<User> userRepository, IJwtProvider jwtProvider)
        {
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            var users = await _userRepository.FindAsync(u => u.Email == request.Email);
            var user = users.FirstOrDefault();

            if (user == null)
            {
                return new LoginResponseDto { IsSuccess = false, Message = "Credenciales incorrectas" };
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return new LoginResponseDto { IsSuccess = false, Message = "Credenciales incorrectas" };
            }

            if (!user.IsEmailVerified)
            {
                return new LoginResponseDto { IsSuccess = false, Message = "Por favor, verifica tu correo electronico antes de iniciar sesion" };
            }

            var token = _jwtProvider.GenerateToken(user);

            return new LoginResponseDto
            {
                IsSuccess = true,
                Message = "Inicio de sesion correcto",
                Token = token,
                FirstName = user.Name,
                Role = user.Role.ToString(),
            };
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
        {   
            // Verificar si el usuario ya existe en el sistema
            var existingUsers = await _userRepository.FindAsync(u => u.Email == request.Email);
            if (existingUsers.Any())
            {
                return new AuthResponseDto { Success = false, Message = "El correo ya esta registrado. " };
            }

            // Crear el usuario con la contraseña encriptada y un token de verificaion
            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Lastname = request.LastName,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = UserRole.Customer, //Por defecto todos son clientes
                CreatedAt = DateTime.UtcNow,
                IsEmailVerified = false,
                VerificationToken = Guid.NewGuid().ToString("N"), //Generacion del token
            };

            await _userRepository.AddAsync(newUser);

            //Logica llamando al servicio de Email
            Console.WriteLine($"[Email Simulado] Por favor verifica tu cuenta con este token {newUser.VerificationToken}");

            return new AuthResponseDto
            {
                Success = true,
                Message = "Registro Exitoso. Por favor revisa tu correo para verificar cuenta"
            };
        }

        public async Task<AuthResponseDto> VerifyEmailAsync(string token)
        {
            var users = await _userRepository.FindAsync(u => u.VerificationToken == token);
            var user = users.FirstOrDefault();

            if (user == null)
            {
                return new AuthResponseDto { Success = false, Message = "Token invalido" };
            }

            if (user.IsEmailVerified)
            {
                return new AuthResponseDto { Success = false, Message = "El correo ya ha asido verificado " };
            }

            user.IsEmailVerified = true;
            _userRepository.Update(user);

            return new AuthResponseDto { Success = true, Message = "Correo verificado correctamente. Ya puedes iniciar sesion" };
        }
    }
}
