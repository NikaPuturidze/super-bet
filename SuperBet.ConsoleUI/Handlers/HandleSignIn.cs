using Ardalis.GuardClauses;
using Spectre.Console;
using SuperBet.Core.Guards;
using SuperBet.Core.Interfaces;
using SuperBet.Core.Session;
using SuperBet.Core.Utils;
using SuperBet.Data.Repositories;

namespace SuperBet.ConsoleUI.Handlers
{
    public class HandleSignIn(
        UserRepository _userRepository,
        SessionManager _sessionManager) : IMenuHandler
    {
        public void Execute()
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

            bool proceed = AnsiConsole.Confirm("Do you want to be remembered on this device?");

            try
            {
                var user = _userRepository.ValidateCredentials(email, password)
                   ?? throw new ArgumentException("Email or password incorrect!");

                _userRepository.ClearRememberedUsers();
                user.IsRemembered = proceed;
                _userRepository.Save();
                _sessionManager.SignIn(user);
            }
            catch (Exception ex)
            {
                AnsiConsole.Markup($"[red]{ex.Message}[/]");
            }
        }
    }
}
