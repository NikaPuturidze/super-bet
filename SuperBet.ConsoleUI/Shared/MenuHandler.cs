using Spectre.Console;
using SuperBet.Core.Interfaces;
using System.Text;

namespace SuperBet.ConsoleUI.Shared
{
    public abstract class MenuHandler<T> : IMenuHandler where T : Enum
    {
        protected readonly List<T> Options = [];
        protected abstract Dictionary<T, string> Labels { get; }
        protected abstract bool HandleChoice(T option);
        protected virtual void BuildMenu()
        {
            Options.Clear();
            Options.AddRange(Labels.Keys);
        }

        protected static T ShowMenu(string title, Dictionary<T, string> options)
        {
            Console.OutputEncoding = Encoding.UTF8;
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[Red]SuperBet![/]\n");

            var choiceLabel = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"[yellow]{title}[/]")
                    .AddChoices(options.Values)
            );

            return options.First(x => x.Value == choiceLabel).Key;
        }

        public virtual void Execute()
        {
            bool exit = false;
            while (!exit)
            {
                BuildMenu();

                var menuDictionary = Options.ToDictionary(
                    option => option,
                    option => Labels[option]
                );

                var selected = ShowMenu("Select an option:", menuDictionary);

                exit = !HandleChoice(selected);
            }
        }

    }
}
