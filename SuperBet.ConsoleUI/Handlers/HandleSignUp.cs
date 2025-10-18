using Ardalis.GuardClauses;
using Spectre.Console;
using SuperBet.Core.Guards;
using SuperBet.Core.Interfaces;
using SuperBet.Core.Models;
using SuperBet.Core.Utils;
using SuperBet.Data.Repositories;

namespace SuperBet.ConsoleUI.Handlers
{
    public class HandleSignUp(
        UserRepository _userRepository) : IMenuHandler
    {
        public void Execute()
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[Red]Sign up to your account[/]\n");

            string username = AnsiConsole.Prompt(
                new TextPrompt<string>("[green]Enter your username:[/]")
                    .Validate(value =>
                    {
                        var error = GuardHelpers.ValidateField(() => Guard.Against.InvalidUsername(value));
                        return error is null ? ValidationResult.Success() : ValidationResult.Error($"[red]{error}[/]");
                    }));

            int age = AnsiConsole.Prompt(
                new TextPrompt<int>("[green]Enter your age:[/]")
                    .Validate(value =>
                    {
                        var error = GuardHelpers.ValidateField(() => Guard.Against.InvalidAge(value));
                        return error is null ? ValidationResult.Success() : ValidationResult.Error($"[red]{error}[/]");
                    }));

            string email = AnsiConsole.Prompt(
                new TextPrompt<string>("[green]Enter your email:[/]")
                    .Validate(value =>
                    {
                        var error = GuardHelpers.ValidateField(() => Guard.Against.InvalidEmail(value));
                        return error is null ? ValidationResult.Success() : ValidationResult.Error($"[red]{error}[/]");
                    }));

            string password = AnsiConsole.Prompt(
                new TextPrompt<string>("[green]Enter your password:[/]")
                    .Validate(value =>
                    {
                        var error = GuardHelpers.ValidateField(() => Guard.Against.InvalidPassword(value));
                        return error is null ? ValidationResult.Success() : ValidationResult.Error($"[red]{error}[/]");
                    }));

            string confirmPassword = AnsiConsole.Prompt(
                new TextPrompt<string>("[green]Confirm your password:[/]")
                    .Validate(value =>
                    {
                        var error = GuardHelpers.ValidateField(() => Guard.Against.PasswordsDoNotMatch(value, password));
                        return error is null ? ValidationResult.Success() : ValidationResult.Error($"[red]{error}[/]");
                    }));

            bool proceed = AnsiConsole.Confirm("Do you acknowledge that you can [red]Lose[/] all your money here?");

            if (!proceed)
            {
                AnsiConsole.MarkupLine("[red]Action canceled.[/]");
                Console.ReadKey(true);
                return;
            }

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
                AnsiConsole.MarkupLine("[green]Account created successfully![/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
            }

            AnsiConsole.MarkupLine("\nPress any key to return to the menu...");
            Console.ReadKey(true);
        }
    }
}
