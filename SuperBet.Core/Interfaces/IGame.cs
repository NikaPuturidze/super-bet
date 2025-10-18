using SuperBet.Core.Models;
using SuperBet.Core.Models.Enums;

namespace SuperBet.Core.Interfaces
{
    public interface IGame
    {
        PlayResult PlayGame(List<string> results, decimal betAmount);
    }
}
