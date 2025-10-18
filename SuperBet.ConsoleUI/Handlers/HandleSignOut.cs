using SuperBet.Core.Interfaces;
using SuperBet.Core.Session;
using SuperBet.Data.Repositories;

namespace SuperBet.ConsoleUI.Handlers
{
    public class HandleSignOut(
        UserRepository _userRepository,
        SessionManager _sessionManager) : IMenuHandler
    {
        public void Execute()
        {
            var user = _sessionManager.CurrentUser;

            if (user != null)
            {
                user.IsRemembered = false;
                _userRepository.Save();
            }

            _sessionManager.SignOut();
        }
    }
}
