using SuperBet.ConsoleUI.Handlers.GamesCatalog;
using SuperBet.ConsoleUI.Shared;
using SuperBet.Core.Interfaces;
using SuperBet.Core.Models.Enums;
using SuperBet.Core.Session;
using SuperBet.Data.Repositories;

namespace SuperBet.ConsoleUI.Handlers
{
    public class HandleMainMenu(
        SessionManager _sessionManager,
        UserRepository _userRepository,
        PlayResultsRepository _playResultsRepository) : MenuHandler<MainMenuOption>
    {
        protected override Dictionary<MainMenuOption, string> Labels { get; } = new()
        {
            { MainMenuOption.GamesCatalog, "🎰 Games Catalog" },
            { MainMenuOption.Deposit, "💳 Deposit" },
            { MainMenuOption.Withdraw, "💵 Withdraw" },
            { MainMenuOption.ViewBalance, "💰 View Balance" },
            { MainMenuOption.SignOut, "🚪 Sign Out" },
            { MainMenuOption.SignUp, "🔑 Sign Up" },
            { MainMenuOption.SignIn, "🔑 Sign In" },
            { MainMenuOption.Exit, "❌ Exit" }
        };

        protected override void BuildMenu()
        {
            Options.Clear();
            if (_sessionManager.IsLoggedIn)
            {
                Options.Add(MainMenuOption.GamesCatalog);
                Options.Add(MainMenuOption.Deposit);
                Options.Add(MainMenuOption.Withdraw);
                Options.Add(MainMenuOption.ViewBalance);
                Options.Add(MainMenuOption.SignOut);
            }
            else
            {
                Options.Add(MainMenuOption.SignUp);
                Options.Add(MainMenuOption.SignIn);
            }
            Options.Add(MainMenuOption.Exit);
        }


        protected override bool HandleChoice(MainMenuOption option)
        {
            IMenuHandler? handler = option switch
            {
                MainMenuOption.GamesCatalog => new HandleGamesCatalog(_sessionManager, _userRepository, _playResultsRepository),
                MainMenuOption.SignUp => new HandleSignUp(_userRepository),
                MainMenuOption.SignIn => new HandleSignIn(_userRepository, _sessionManager),
                MainMenuOption.SignOut => new HandleSignOut(_userRepository, _sessionManager),
                MainMenuOption.Exit => null,
                _ => throw new NotImplementedException()
            };

            if (handler is null) return false;

            handler.Execute();
            return true;
        }

    }
}
