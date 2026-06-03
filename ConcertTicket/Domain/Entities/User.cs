using ConcertTicket.Domain.Enums;

namespace ConcertTicket.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsEmailVerified { get; set; } = false;
        public string VerificationToken { get; set; }

        //Enum
        public UserRole Role { get; set; }
        //Relaciones
        public ICollection<Reservation> Reservations { get; set; }

    }


}
