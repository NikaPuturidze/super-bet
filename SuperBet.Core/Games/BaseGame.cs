using SuperBet.Core.Interfaces;
using SuperBet.Core.Models;

namespace SuperBet.Core.Games
{
    public abstract class BaseGame : IGame
    {
        public abstract PlayResult PlayGame(List<string> results, decimal betAmount);
    }
}
