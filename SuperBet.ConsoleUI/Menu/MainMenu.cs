using Ardalis.GuardClauses;
using Spectre.Console;
using SuperBet.Core.Guards;
using SuperBet.Core.Interfaces;
using SuperBet.Core.Models;
using SuperBet.Core.Models.Enums;
using SuperBet.Core.Session;
using SuperBet.Core.Utils;
using SuperBet.Data.Repositories;
using System.Text;

namespace SuperBet.ConsoleUI.Menu
{
    internal class MainMenu(
        SessionManager _sessionManager,
        UserRepository _userRepository
    ) : IMainMenu
    {
        private readonly List<MainMenuOption> MenuOptions = [];

        private readonly Dictionary<MainMenuOption, string> MenuLabels = new()
        {
            { MainMenuOption.PlayGame, "🎰 Play Game" },
            { MainMenuOption.Deposit, "💳 Deposit" },
            { MainMenuOption.Withdraw, "💵 Withdraw" },
            { MainMenuOption.ViewBalance, "💰 View Balance" },
            { MainMenuOption.SignOut, "🚪 Sign Out" },
            { MainMenuOption.SignUp, "🔑 Sign Up" },
            { MainMenuOption.SignIn, "🔑 Sign In" },
            { MainMenuOption.Exit, "❌ Exit" }
        };

        public void RenderMenu()
        {
            while (true) {
                Console.OutputEncoding = Encoding.UTF8;

                AnsiConsole.Clear();
                AnsiConsole.MarkupLine("[Red]Welcome to SuperBet![/]\n");

                BuildMenu();

                var displayChoices = MenuOptions
                    .Select(o => MenuLabels[o])
                    .ToList();

                var choiceLabel = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("[yellow]Use [cyan]Arrows[/] to navigate and press [cyan]Enter[/] to select:[/]")
                    .AddChoices(displayChoices)
                );

                var selectedOption = MenuLabels.First(x => x.Value == choiceLabel).Key;

                HandleChoice(selectedOption);
            }
        }

        public void BuildMenu()
        {
            MenuOptions.Clear();

            if (_sessionManager.IsLoggedIn)
            {
                MenuOptions.AddRange(
                [
                    MainMenuOption.PlayGame,
                    MainMenuOption.Deposit,
                    MainMenuOption.Withdraw,
                    MainMenuOption.ViewBalance,
                    MainMenuOption.SignOut,
                    MainMenuOption.Exit
                ]);
            }
            else
            {
                MenuOptions.AddRange(
                [
                    MainMenuOption.SignUp,
                    MainMenuOption.SignIn,
                    MainMenuOption.Exit
                ]);
            }
        }

        public void HandleChoice(MainMenuOption option)
        {
            switch (option)
            {
                case MainMenuOption.SignUp:
                    HandleSignUp();
                    break;

                case MainMenuOption.SignIn:
                    HandleSignIn();
                    break;

                case MainMenuOption.Exit:
                    Environment.Exit(0);
                    break;
            }
        }

        public void HandleSignUp()
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[Red]Sign up to your account[/]\n");

            string username = AnsiConsole.Prompt(
                new TextPrompt<string>("[green]Enter your username:[/]")
                    .Validate(value =>
                    {
                        var error = GuardHelpers.ValidateField(() =>
                        {
                            Guard.Against.InvalidUsername(value);
                        });
                        
                        return error is null 
                            ? ValidationResult.Success() 
                            : ValidationResult.Error($"[red]{error}[/]");
                    })
            );

            int age = AnsiConsole.Prompt(
                new TextPrompt<int>("[green]Enter your age:[/]")
                    .Validate(value =>
                    {
                        var error = GuardHelpers.ValidateField(() =>
                        {
                            Guard.Against.InvalidAge(value);
                        });

                        return error is null ? ValidationResult.Success() : ValidationResult.Error($"[red]{error}[/]");

                    })

            );

            string email = AnsiConsole.Prompt(
                new TextPrompt<string>("[green]Enter your email:[/]")
                    .Validate(value =>
                    {
                        var error = GuardHelpers.ValidateField(() =>
                        {
                            Guard.Against.InvalidEmail(value);
                        });

                        return error is null ? ValidationResult.Success() : ValidationResult.Error($"[red]{error}[/]");

                    })

            );

            string password = AnsiConsole.Prompt(
               new TextPrompt<string>("[green]Enter your password:[/]")
                   .Validate(value =>
                   {
                       var error = GuardHelpers.ValidateField(() =>
                       {
                           Guard.Against.InvalidPassword(value);
                       });

                       return error is null ? ValidationResult.Success() : ValidationResult.Error($"[red]{error}[/]");

                   })

            );

            string confrimPassword = AnsiConsole.Prompt(
               new TextPrompt<string>("[green]Confirm your password:[/]")
                   .Validate(value =>
                   {
                       var error = GuardHelpers.ValidateField(() =>
                       {
                           Guard.Against.PasswordsDoNotMatch(value, password);
                       });

                       return error is null ? ValidationResult.Success() : ValidationResult.Error($"[red]{error}[/]");

                   })

            );

            bool proceed = AnsiConsole.Confirm("Do you acknowledge that you can [red]Lose[/] all your money here?");

            if (proceed) AnsiConsole.MarkupLine("[green]Okay, proceeding...[/]");
            else AnsiConsole.MarkupLine("[red]Action canceled.[/]");

            try
            {
                _userRepository.Add(new User
                {
                    Username = username,
                    Age = age,
                    Email = email,
                    PasswordHash = password,
                });

                _userRepository.Save();
            }
            catch (Exception ex)
            {
                AnsiConsole.Markup($"[red]{ex.Message}[/]");
            }


            AnsiConsole.MarkupLine("\nPress any key to return to the menu...");
            Console.ReadKey(true);
        }

        public void HandleSignIn()
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[Red]Sign in to your account[/]\n");

            string email = AnsiConsole.Prompt(
               new TextPrompt<string>("[green]Enter your email:[/]")
                   .Validate(value =>
                   {
                       var error = GuardHelpers.ValidateField(() =>
                       {
                            Guard.Against.InvalidEmail(value);
                       });

                       return error is null ? ValidationResult.Success() : ValidationResult.Error($"[red]{error}[/]");
                   })
            );

            string password = AnsiConsole.Prompt(new TextPrompt<string>("[green]Enter your password:[/]"));

            try
            {
                var user = _userRepository.ValidateCredentials(email, password) ?? throw new ArgumentException("Email or Password incorrect!");
                _sessionManager.SignIn(user);
            }
            catch (Exception ex)
            {
                AnsiConsole.Markup($"[red]{ex.Message}[/]");
            }


            AnsiConsole.MarkupLine("\nPress any key to return to the menu...");
            Console.ReadKey(true);
        }
    }
}
