namespace SuperBet.Core.Models
{
    public class PlayResult
    {
        public int UserId { get; set; }
        public string GameName { get; set; } = string.Empty;
        public decimal BetAmount { get; set; }
        public decimal Payout { get; set; }
        public decimal NetGain { get; set; }
        public bool IsWin { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public Dictionary<string, object?>? Metadata { get; set; }
    }
}
