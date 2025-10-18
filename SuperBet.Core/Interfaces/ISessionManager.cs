using SuperBet.Core.Models;

namespace SuperBet.Core.Interfaces
{
    public interface ISessionManager
    {
        User? CurrentUser { get; }
        bool IsLoggedIn { get; }
        void SignIn(User user);
        void SignOut();
    }
}
