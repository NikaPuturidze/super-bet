using Spectre.Console;
using SuperBet.Core.Games.Slots;
using SuperBet.Core.Interfaces;
using SuperBet.Core.Session;
using SuperBet.Data.Repositories;

namespace SuperBet.ConsoleUI.Handlers.GamesCatalog.Slots
{
    public class HandleSlots(
        SessionManager _sessionManager,
        UserRepository _userRepository,
        PlayResultsRepository _playResultsRepository) : IMenuHandler
    {
        public void Execute()
        {
            AnsiConsole.Clear();

            var user = _sessionManager.CurrentUser;
            if (user == null)
            {
                AnsiConsole.MarkupLine("[red]No user session found.[/]");
                return;
            }

            decimal balance = user.Balance;
            decimal betAmount = AskForBet(balance);

            var slot = new SlotsLogic();
            bool exit = false;

            while (!exit)
            {
                AnsiConsole.Clear();
                AnsiConsole.MarkupLine($"[yellow]Your current balance:[/] [green]{balance:C}[/]");
                AnsiConsole.MarkupLine($"[yellow]Current bet:[/] [cyan]{betAmount:C}[/]\n");

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[bold yellow]Choose an option:[/]")
                        .AddChoices("🎰 Spin", "💰 Change Bet", "🚪 Exit")
                );

                switch (choice)
                {
                    case "🎰 Spin":
                        if (balance < betAmount)
                        {
                            AnsiConsole.MarkupLine("[red]You don't have enough balance to spin.[/]");
                            AnsiConsole.MarkupLine("\nPress any key to return...");
                            Console.ReadKey(true);
                            continue;
                        }

                        var result = slot.SpinReels();
                        var outcome = slot.PlayGame(result, betAmount);

                        _playResultsRepository.Add(outcome);
                        user = _userRepository.UpdateBalance(user.Id, outcome.NetGain)!;
                        balance = user.Balance;


                        AnsiConsole.MarkupLine("\n[green]Spinning the reels...[/]");
                        AnsiConsole.MarkupLine($"\n[bold yellow]{string.Join(" | ", result)}[/]");

                        if (outcome.IsWin)
                        {
                            AnsiConsole.MarkupLine($"\n[bold green]Congratulations! You won {outcome.Payout:C}![/]");
                            AnsiConsole.MarkupLine($"[yellow]Net gain:[/] [green]+{outcome.NetGain:C}[/]");
                        }
                        else
                        {
                            AnsiConsole.MarkupLine($"\n[bold red]No win this time. You lost {betAmount:C}.[/]");
                        }

                        AnsiConsole.MarkupLine($"\n[grey]New balance:[/] [green]{balance:C}[/]");
                        AnsiConsole.MarkupLine("\nPress any key to continue...");
                        Console.ReadKey(true);
                        break;

                    case "💰 Change Bet":
                        betAmount = AskForBet(balance);
                        break;

                    case "🚪 Return":
                        exit = true;
                        break;
                }
            }
        }

        private static decimal AskForBet(decimal balance)
        {
            decimal betAmount;

            while (true)
            {
                betAmount = AnsiConsole.Ask<decimal>("\nEnter your [green]bet amount[/]: ");
                if (betAmount <= 0)
                {
                    AnsiConsole.MarkupLine("[red]Bet amount must be greater than zero.[/]");
                    continue;
                }
                if (betAmount > balance)
                {
                    AnsiConsole.MarkupLine("[red]You don't have enough balance for that bet.[/]");
                    continue;
                }
                break;
            }

            return betAmount;
        }
    }
}
