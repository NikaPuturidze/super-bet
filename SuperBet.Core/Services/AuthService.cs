using SuperBet.Core.Interfaces;
using SuperBet.Core.Session;

namespace SuperBet.Core.Services
{
    public class AuthService(SessionManager sessionManager) : IAuthService
    {
        private readonly ISessionManager _sessionManager = sessionManager;

        public bool SignIn(string username, string password)
        {
            throw new NotImplementedException();
        }

        public bool SignUp(string username, string password, string confirmPassword)
        {
            throw new NotImplementedException();
        }

        public bool SignOut()
        {
            throw new NotImplementedException();
        }
    }
}
