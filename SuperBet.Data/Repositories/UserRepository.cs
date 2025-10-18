using Ardalis.GuardClauses;
using SuperBet.Core.Guards;
using SuperBet.Core.Interfaces;
using SuperBet.Core.Models;
using SuperBet.Core.Session;

namespace SuperBet.Data.Repositories
{
    public class UserRepository(DataContext _context, SessionManager _sessionManager) : IUserRepository
    {
        public void Add(User user)
        {
            Guard.Against.DuplicateEmail(user.Email, _context.Users);
            _context.Users.Add(user);
        }

        public List<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public User? GetByEmail(string email)
        {
            return _context.Users.Where(u => u.Email.Equals(email)).FirstOrDefault();
        }

        public User? ValidateCredentials(string email, string password)
        {
            var user = GetByEmail(email);

            if (user == null) return null;
            if (password == null) return null;
            if (user.PasswordHash == password) return user;

            return null;
        }

        public void Save()
        {
            _context.SaveData();
        }

        public void Update(User user)
        {
            throw new NotImplementedException();
        }

        public User? UpdateBalance(decimal amount)
        {
            var user = _sessionManager.CurrentUser;
            if (user == null) return null;
            user.Balance += amount;
            _context.SaveData();
            return user;
        }

        public User? GetRememberedUser()
        {
            return _context.Users.FirstOrDefault(u => u.IsRemembered);
        }

        public void ClearRememberedUsers()
        {
            foreach (var u in _context.Users)
            {
                u.IsRemembered = false;
            }
            _context.SaveData();
        }

    }
}
