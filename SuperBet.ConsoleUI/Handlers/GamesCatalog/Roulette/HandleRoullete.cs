using Spectre.Console;
using SuperBet.ConsoleUI.Shared;
using SuperBet.Core.Interfaces;
using SuperBet.Core.Models.Enums;
using SuperBet.Core.Session;
using SuperBet.Data.Repositories;

namespace SuperBet.ConsoleUI.Handlers.GamesCatalog.Roulette
{
    public class HandleRouletteMenu(
          SessionManager _sessionManager,
          UserRepository _userRepository,
          PlayResultsRepository _playResultsRepository
      ) : MenuHandler<RouletteMenuOption>
    {
        protected override Dictionary<RouletteMenuOption, string> Labels { get; } = new()
        {
            { RouletteMenuOption.Straight, "🎯 Straight (specific number)" },
            { RouletteMenuOption.Color, "🎨 Color (Red/Black/Green)" },
            { RouletteMenuOption.OddEven, "⚖️ Odd / Even" },
            { RouletteMenuOption.LowHigh, "⬆️ Low / High (1–18 / 19–36)" },
            { RouletteMenuOption.Dozen, "🔢 Dozen (1–12 / 13–24 / 25–36)" },
            { RouletteMenuOption.Column, "🏛️ Column (1st / 2nd / 3rd)" },
            { RouletteMenuOption.Return, "↩️ Return" }
        };

        protected override void BuildMenu()
        {
            Options.Clear();
            Options.AddRange(Enum.GetValues<RouletteMenuOption>());
        }

        public override void Execute()
        {
            var user = _sessionManager.CurrentUser;
            if (user == null)
            {
                AnsiConsole.MarkupLine("[red]No user session found.[/]");
                return;
            }

            decimal balance = user.Balance;

            bool exit = false;

            while (!exit)
            {
                AnsiConsole.Clear();

                AnsiConsole.MarkupLine($"[bold green]Roulette[/]\n");
                AnsiConsole.MarkupLine($"[yellow]Current balance:[/] [green]{user.Balance:C}[/]\n");
                ShowRouletteTable();

                BuildMenu();
                var menuDictionary = Options.ToDictionary(o => o, o => Labels[o]);

                AnsiConsole.MarkupLine("[bold yellow]\nSelect your bet type:[/]");
                var selected = AnsiConsole.Prompt(
                    new SelectionPrompt<RouletteMenuOption>()
                        .PageSize(10)
                        .AddChoices(menuDictionary.Keys)
                        .UseConverter(option => menuDictionary[option])
                );

                exit = !HandleChoice(selected);
            }
        }

        protected override bool HandleChoice(RouletteMenuOption option)
        {
            IMenuHandler? handler = option switch
            {
                RouletteMenuOption.Return => null,
                _ => new HandleRouletteBet(_sessionManager, _userRepository, _playResultsRepository, option)
            };

            if (handler is null)
                return false;

            handler.Execute();
            return true;
        }

        private static void ShowRouletteTable()
        {
            var redNumbers = new HashSet<int>
            {
                1,3,5,7,9,12,14,16,18,19,21,23,25,27,30,32,34,36
            };

            var grid = new Grid();
            grid.AddColumn();
            grid.AddColumn();
            grid.Expand();

            var zeroTable = new Table().HideHeaders().Border(TableBorder.Rounded);
            zeroTable.AddColumn("0");
            zeroTable.AddRow("[green]0[/]");
            var zeroPanel = new Panel(zeroTable).Header("[bold]Zero[/]", Justify.Center);

            var table = new Table().Border(TableBorder.Rounded).Centered();
            table.Title("[bold yellow]Roulette[/]");
            table.AddColumn(new TableColumn("[bold]1st[/]").Centered());
            table.AddColumn(new TableColumn("[bold]2nd[/]").Centered());
            table.AddColumn(new TableColumn("[bold]3rd[/]").Centered());

            for (int row = 12; row >= 1; row--)
            {
                int n1 = 3 * row - 2;
                int n2 = 3 * row - 1;
                int n3 = 3 * row;
                table.AddRow(
                    FormatCell(n1, redNumbers.Contains(n1)),
                    FormatCell(n2, redNumbers.Contains(n2)),
                    FormatCell(n3, redNumbers.Contains(n3))
                );
            }

            grid.AddRow(zeroPanel, table);
            AnsiConsole.Write(grid);
            AnsiConsole.MarkupLine("\n[red]■[/] Red   [white]■[/] Black   [green]■[/] Zero\n");
        }

        private static string FormatCell(int num, bool isRed)
        {
            string color = num == 0 ? "green" : isRed ? "red" : "white";
            return $"[{color}]{num}[/]";
        }
    }
}
