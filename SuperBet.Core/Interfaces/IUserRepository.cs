using SuperBet.Core.Models;

namespace SuperBet.Core.Interfaces
{
    public interface IUserRepository
    {
        List<User> GetAll();
        User? GetByEmail(string email);
        User? ValidateCredentials(string email, string password);
        void Add(User user);
        void Update(User user);
        User? UpdateBalance(decimal amount);
        void Save();
        User? GetRememberedUser();
        void ClearRememberedUsers();
    }
}
