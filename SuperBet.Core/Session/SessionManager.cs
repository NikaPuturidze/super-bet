using SuperBet.Core.Interfaces;
using SuperBet.Core.Models;

namespace SuperBet.Core.Session
{
    public class SessionManager : ISessionManager
    {
        public User? CurrentUser { get; private set; }
        public bool IsLoggedIn => CurrentUser != null;

        public void SignIn(User user) => CurrentUser = user;
        public void SignOut() => CurrentUser = null;
    }
}
