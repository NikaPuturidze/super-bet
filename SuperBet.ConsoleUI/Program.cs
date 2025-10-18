using SuperBet.ConsoleUI.Handlers;
using SuperBet.Core.Session;
using SuperBet.Data;
using SuperBet.Data.Repositories;

namespace SuperBet.ConsoleUI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var context = new DataContext();
            context.LoadData();
            var sessionManager = new SessionManager();
            var userRepository = new UserRepository(context);
            var playResultsRepository = new PlayResultsRepository(context);

            var rememberedUser = userRepository.GetRememberedUser();
            if (rememberedUser != null)
                sessionManager.SignIn(rememberedUser);

            var menu = new HandleMainMenu(sessionManager, userRepository, playResultsRepository);
            menu.Execute();
        }
    }
}
