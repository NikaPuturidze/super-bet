using Ardalis.GuardClauses;
using Spectre.Console;
using SuperBet.Core.Interfaces;
using SuperBet.Core.Session;

namespace SuperBet.ConsoleUI.Handlers
{
    internal class HandleViewBalance(SessionManager _sessionManager) : IMenuHandler
    {
        public void Execute()
        {
            AnsiConsole.Clear();

            Guard.Against.Null(_sessionManager.CurrentUser, nameof(_sessionManager.CurrentUser), "No user session found.");

            decimal currentBalance = _sessionManager.CurrentUser.Balance;

            AnsiConsole.MarkupLine($"[yellow]Your current balance is:[/] [green]{currentBalance:C}[/]\n");

            AnsiConsole.MarkupLine("[grey]Press any key to return...[/]");

            Console.ReadKey(true);
        }
    }
}
