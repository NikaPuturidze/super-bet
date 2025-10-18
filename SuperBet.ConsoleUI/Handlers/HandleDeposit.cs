using Ardalis.GuardClauses;
using Spectre.Console;
using SuperBet.Core.Interfaces;
using SuperBet.Core.Session;
using SuperBet.Data.Repositories;

namespace SuperBet.ConsoleUI.Handlers
{
    public class HandleDeposit(UserRepository _userRepository, SessionManager _sessionManager) : IMenuHandler
    {
        public void Execute()
        {
            AnsiConsole.Clear();

            Guard.Against.Null(_sessionManager.CurrentUser, nameof(_sessionManager.CurrentUser), "No user session found.");

            decimal currentBalance = _sessionManager.CurrentUser.Balance;

            AnsiConsole.MarkupLine($"[yellow]Your current balance is:[/] [green]{currentBalance:C}[/]\n");

            decimal amount = AnsiConsole.Prompt(
                new TextPrompt<decimal>("[green]Enter deposit amount:[/]")
                    .Validate(value =>
                    {
                        return value > 0 ? ValidationResult.Success() : ValidationResult.Error("[red]Amount must be greater than zero.[/]");
                    })
            );

            _userRepository.UpdateBalance(amount);

            AnsiConsole.Status()
                .Start("Processing deposit...", ctx =>
                {
                    Thread.Sleep(1000);
                });

            AnsiConsole.Clear();

            AnsiConsole.MarkupLine($"[green]Successfully deposited {amount:C} to your account![/]");
            AnsiConsole.MarkupLine("[grey]Press any key to return...[/]");

            Console.ReadKey(true);
        }
    }
}
