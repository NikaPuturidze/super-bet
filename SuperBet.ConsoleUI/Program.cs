using SuperBet.ConsoleUI.Menu;
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
            var sessionManager = new SessionManager();
            var userRepository = new UserRepository(context);
            var menu = new MainMenu(sessionManager, userRepository);
            context.LoadData();
            menu.RenderMenu();
        }
    }
}
