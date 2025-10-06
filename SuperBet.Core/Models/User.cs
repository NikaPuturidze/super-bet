using SuperBet.Core.Models.Enums;

namespace SuperBet.Core.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int Age { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public List<UserTransaction>? Transactions { get; set; }
        public UserRole Role { get; set; } = UserRole.User;
        public decimal Balance { get; set; } = 0;
        public int FreeSpins { get; set; } = 0;
    }
}
