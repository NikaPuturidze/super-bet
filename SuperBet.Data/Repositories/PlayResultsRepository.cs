using SuperBet.Core.Interfaces;
using SuperBet.Core.Models;

namespace SuperBet.Data.Repositories
{
    public class PlayResultsRepository(DataContext _context) : IPlayResultsRepository
    {
        public void Add(PlayResult playResult)
        {
            _context.PlayResults.Add(playResult);
            _context.SaveData();
        }

        public void Save()
        {
            _context.SaveData();
        }
    }
}
