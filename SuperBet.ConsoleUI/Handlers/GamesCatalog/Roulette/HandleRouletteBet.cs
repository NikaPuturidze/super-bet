using Spectre.Console;
using SuperBet.Core.Games.Roulette;
using SuperBet.Core.Interfaces;
using SuperBet.Core.Models.Enums;
using SuperBet.Core.Session;
using SuperBet.Data.Repositories;

namespace SuperBet.ConsoleUI.Handlers.GamesCatalog.Roulette
{
    public class HandleRouletteBet(
           SessionManager _sessionManager,
           UserRepository _userRepository,
           PlayResultsRepository _playResultsRepository,
           RouletteMenuOption _option
       ) : IMenuHandler
    {
        public void Execute()
        {
            var user = _sessionManager.CurrentUser;
            if (user == null)
            {
                AnsiConsole.MarkupLine("[red]No user session found.[/]");
                return;
            }

            decimal balance = user.Balance;

            AnsiConsole.Clear();

            AnsiConsole.MarkupLine($"[bold green]Roulette[/]\n");
            AnsiConsole.MarkupLine($"[yellow]Current balance:[/] [green]{user.Balance:C}[/]\n");

            string betValue = GetValidatedBetInput(_option);
            decimal betAmount = GetValidatedBetAmount(_sessionManager.CurrentUser?.Balance ?? 0);
            var logic = new RouletteLogic(_sessionManager);
            var result = logic.PlayGame([betValue], betAmount);
            _userRepository.UpdateBalance(result.NetGain);

            AnsiConsole.Status().Spinner(Spinner.Known.Dots).Start("Spinning wheel...", _ =>
            {
                Thread.Sleep(1500);
            });

            var number = (int)result.Metadata!["Number"]!;
            var color = result.Metadata!["Color"]!.ToString();
            var outcome = result.IsWin ? "[green]WIN[/]" : "[red]LOSE[/]";

            AnsiConsole.MarkupLine(
                $"\n[bold]Ball landed on:[/] [yellow]{number}[/] " +
                $"({(color == "black" ? "[white]Black[/]" : $"[{color}]{char.ToUpper(color![0]) + color[1..]}[/]")})"
            );
            AnsiConsole.MarkupLine($"[bold]Your Bet:[/] {betValue}");
            AnsiConsole.MarkupLine($"[bold]Bet Amount:[/] {betAmount:C}");
            AnsiConsole.MarkupLine($"[bold]Result:[/] {outcome}");
            AnsiConsole.MarkupLine($"[bold]Payout:[/] {result.Payout:C}");
            AnsiConsole.MarkupLine($"[bold]Net Gain:[/] {(result.NetGain >= 0 ? "[green]" : "[red]")}{result.NetGain:C}[/]");

            _playResultsRepository.Add(result);

            AnsiConsole.MarkupLine("\n[dim]Press any key to return to Roulette Menu...[/]");
            Console.ReadKey(true);
        }

        private static string GetValidatedBetInput(RouletteMenuOption option)
        {
            while (true)
            {
                try
                {
                    return option switch
                    {
                        RouletteMenuOption.Straight => ValidateNumberInput(),
                        RouletteMenuOption.Color => AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("[yellow]Choose color:[/]")
                                .AddChoices("Red", "Black", "Green")),
                        RouletteMenuOption.OddEven => AnsiConsole.Prompt(
                            new SelectionPrompt<string>().Title("[yellow]Choose:[/]").AddChoices("Odd", "Even")),
                        RouletteMenuOption.LowHigh => AnsiConsole.Prompt(
                            new SelectionPrompt<string>().Title("[yellow]Choose range:[/]").AddChoices("Low", "High")),
                        RouletteMenuOption.Dozen => AnsiConsole.Prompt(
                            new SelectionPrompt<string>().Title("[yellow]Choose dozen:[/]").AddChoices("1st", "2nd", "3rd")),
                        RouletteMenuOption.Column => AnsiConsole.Prompt(
                            new SelectionPrompt<string>().Title("[yellow]Choose column:[/]").AddChoices("1st", "2nd", "3rd")),
                        _ => throw new NotImplementedException()
                    };
                }
                catch
                {
                    AnsiConsole.MarkupLine("[red]❌ Invalid input. Try again.[/]");
                }
            }
        }

        private static string ValidateNumberInput()
        {
            while (true)
            {
                var input = AnsiConsole.Ask<string>("[yellow]Enter number (0–36):[/]");
                if (int.TryParse(input, out int num) && num >= 0 && num <= 36)
                    return num.ToString();

                AnsiConsole.MarkupLine("[red]❌ Invalid number. Please enter between 0 and 36.[/]");
            }
        }

        private static decimal GetValidatedBetAmount(decimal balance)
        {
            while (true)
            {
                var amount = AnsiConsole.Ask<decimal>("[yellow]Enter your bet amount:[/]");

                if (amount <= 0)
                {
                    AnsiConsole.MarkupLine("[red]❌ Bet must be greater than 0.[/]");
                    continue;
                }

                if (amount > balance)
                {
                    AnsiConsole.MarkupLine("[red]❌ You don't have enough balance for that bet.[/]");
                    continue;
                }

                return amount;
            }
        }
    }
}
