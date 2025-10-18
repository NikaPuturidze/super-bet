using Spectre.Console;
using SuperBet.Core.Interfaces;

namespace SuperBet.ConsoleUI.Handlers
{
    public class HandleExit : IMenuHandler
    {
        public void Execute()
        {
            AnsiConsole.Clear();
            Environment.Exit(0);
        }
    }
}
