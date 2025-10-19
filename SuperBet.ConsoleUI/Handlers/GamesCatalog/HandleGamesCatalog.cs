using SuperBet.ConsoleUI.Shared;
using SuperBet.Core.Interfaces;
using SuperBet.Core.Models.Enums;
using SuperBet.Core.Session;
using SuperBet.Data.Repositories;

namespace SuperBet.ConsoleUI.Handlers.GamesCatalog
{
    public class HandleGamesCatalog(
        SessionManager _sessionManager,
        UserRepository _userRepository,
        PlayResultsRepository _playResultsRepository) : MenuHandler<GameOption>
    {
        protected override Dictionary<GameOption, string> Labels => new()
        {
            { GameOption.Slots, "🎰 Slots" },
            { GameOption.Roulette, "🎰 Roulette" },
            { GameOption.Return, "↩️ Return" },
        };

        protected override bool HandleChoice(GameOption option)
        {
            IMenuHandler? handler = option switch
            {
                GameOption.Slots => new Slots.HandleSlots(_sessionManager, _userRepository, _playResultsRepository),
                GameOption.Roulette => new Roulette.HandleRouletteMenu(_sessionManager, _userRepository, _playResultsRepository),
                GameOption.Return => null,
                _ => throw new NotImplementedException()
            };

            if (handler is null)  return false;

            handler.Execute();
            return true;
        }

    }
}
