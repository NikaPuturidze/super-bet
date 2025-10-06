using SuperBet.Core.Models.Enums;

namespace SuperBet.Core.Interfaces
{
    public interface IMainMenu
    {
        void RenderMenu();
        void BuildMenu();
        void HandleChoice(MainMenuOption choice);
        void HandleSignUp();
    }
}
