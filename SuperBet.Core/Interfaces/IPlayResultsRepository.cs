using SuperBet.Core.Models;

namespace SuperBet.Core.Interfaces
{
    public interface IPlayResultsRepository
    {
        void Add(PlayResult playResult);
        void Save();
    }
}
