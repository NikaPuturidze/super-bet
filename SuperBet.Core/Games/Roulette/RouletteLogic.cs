using SuperBet.Core.Models;
using SuperBet.Core.Session;

namespace SuperBet.Core.Games.Roulette
{
    public class RouletteLogic(SessionManager _sessionManager) : BaseGame
    {
        private static readonly int[] RedNumbers = {
            1,3,5,7,9,12,14,16,18,19,21,23,25,27,30,32,34,36
        };

        private readonly Random _random = new();

        public override PlayResult PlayGame(List<string> results, decimal betAmount)
        {
            int winningNumber = _random.Next(0, 37);
            string winningColor = GetColor(winningNumber);
            string bet = results[0].Trim().ToLower();

            bool isWin = false;
            decimal multiplier = 0;

            if (TryHandleStraight(bet, winningNumber, ref isWin, ref multiplier)) { }
            else if (TryHandleColor(bet, winningColor, ref isWin, ref multiplier)) { }
            else if (TryHandleOddEven(bet, winningNumber, ref isWin, ref multiplier)) { }
            else if (TryHandleLowHigh(bet, winningNumber, ref isWin, ref multiplier)) { }

            decimal payout = isWin ? betAmount * (multiplier + 1) : 0;
            decimal netGain = payout - betAmount;

            return new PlayResult
            {
            UserId = _sessionManager.CurrentUser!.Id,
            GameName = "Roulette",
            BetAmount = betAmount,
            Payout = payout,
            NetGain = netGain,
            IsWin = isWin,
            Metadata = new Dictionary<string, object?>
            {
                { "Number", winningNumber },
                { "Color", winningColor },
                { "Bet", bet },
                { "Multiplier", multiplier }
            }
            };
        }
        private static bool TryHandleStraight(string bet, int winningNumber, ref bool win, ref decimal multiplier)
        {
            if (int.TryParse(bet, out int betNum) && betNum == winningNumber)
            {
                win = true;
                multiplier = 35;
                return true;
            }
            return false;
        }

        private static bool TryHandleColor(string bet, string winningColor, ref bool win, ref decimal multiplier)
        {
            if (bet is "red" or "black" or "green")
            {
                win = (bet == winningColor);

                multiplier = bet == "green" ? 35 : 1;

                return true;
            }

            return false;
        }


        private static bool TryHandleOddEven(string bet, int winningNumber, ref bool win, ref decimal multiplier)
        {
            if (bet is not ("odd" or "even") || winningNumber == 0)
                return false;

            win = (bet == "even" && winningNumber % 2 == 0)
                || (bet == "odd" && winningNumber % 2 == 1);
            multiplier = 1;
            return true;
        }

        private static bool TryHandleLowHigh(string bet, int winningNumber, ref bool win, ref decimal multiplier)
        {
            if (bet is not ("low" or "high") || winningNumber == 0)
                return false;

            win = (bet == "low" && winningNumber is >= 1 and <= 18)
                || (bet == "high" && winningNumber is >= 19 and <= 36);
            multiplier = 1;
            return true;
        }
        private static string GetColor(int number)
        {
            return number switch
            {
                0 => "green",
                _ when Array.IndexOf(RedNumbers, number) >= 0 => "red",
                _ => "black"
            };
        }
    }
}
