using SuperBet.Core.Models.Enums;

namespace SuperBet.Core.Interfaces
{
    public interface IMainMenu
    {
        void RenderMainMenu();
        void BuildMainMenu();
        void HandleChoice(MainMenuOption choice);
    }
}
