using SuperBet.Core.Models;
using SuperBet.Core.Models.Enums;

namespace SuperBet.Core.Games.Slots
{
    public class SlotsLogic : BaseGame
    {
        private static readonly Dictionary<SlotSymbol, string> symbolIcons = new()
        {
            { SlotSymbol.Cherry, "🍒" },
            { SlotSymbol.Lemon, "🍋" },
            { SlotSymbol.Diamond, "💎" },
            { SlotSymbol.Seven, "7️" },
            { SlotSymbol.Wild, "⭐" }
        };

        private static readonly List<(SlotSymbol Symbol, int number)> thresholdTable =
        [
            (SlotSymbol.Cherry, 30),
            (SlotSymbol.Lemon, 55),
            (SlotSymbol.Diamond, 75),
            (SlotSymbol.Seven, 85),
            (SlotSymbol.Wild, 100)
        ];


        private static readonly Dictionary<SlotSymbol, int> payoutMultipliers = new()
        {
            { SlotSymbol.Cherry, 8 },
            { SlotSymbol.Lemon, 16 },
            { SlotSymbol.Diamond, 50 },
            { SlotSymbol.Seven, 100 },
            { SlotSymbol.Wild, 1000 }
        };

        public List<string> SpinReels(int numberOfReels = 3)
        {
            var random = new Random();
            var results = new List<string>();

            for (int i = 0; i < numberOfReels; i++)
            {
                int spin = random.Next(1, 101);
                foreach (var (symbol, threshold) in thresholdTable)
                {
                    if (spin <= threshold)
                    {
                        results.Add(symbolIcons[symbol]);
                        break;
                    }
                }
            }
            return results;
        }

        public override PlayResult PlayGame(List<string> results, decimal betAmount)
        {
            var outcome = new PlayResult
            {
                GameName = "Slots",
                BetAmount = betAmount,
                Payout = 0,
                NetGain = -betAmount,
                IsWin = false,
                Metadata = new Dictionary<string, object?>
                {
                    { "Results", results }
                }
            };

            if (results.Distinct().Count() == 1)
            {
                var symbol = symbolIcons.FirstOrDefault(x => x.Value == results[0]).Key;

                if (payoutMultipliers.TryGetValue(symbol, out int multiplier))
                {
                    decimal payout = betAmount * multiplier;
                    decimal netGain = payout - betAmount;

                    outcome.Payout = payout;
                    outcome.NetGain = netGain;
                    outcome.IsWin = true;
                    outcome.Metadata!["Symbol"] = symbol.ToString();
                    outcome.Metadata!["Multiplier"] = multiplier;
                }
            }

            return outcome;
        }
    }
}
