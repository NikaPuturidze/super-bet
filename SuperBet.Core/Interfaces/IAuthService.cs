namespace SuperBet.Core.Interfaces
{
    internal interface IAuthService
    {
        bool SignIn(string username, string password);
        bool SignUp(string username, string password, string confirmPassword);
        bool SignOut();
    }
}
