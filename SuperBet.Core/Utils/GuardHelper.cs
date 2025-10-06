namespace SuperBet.Core.Utils
{
    public static class GuardHelpers
    {
        public static string? ValidateField(Action guardAction)
        {
            try
            {
                guardAction();
                return null;
            }
            catch (ArgumentException ex)
            {
                return ex.Message.Split(" (Parameter")[0];
            }
        }
    }
}
