using Hwdtech;
using Scriban;

namespace SpaceBattle.Lib;

public class SomeAdapterBuilder
{
    public void Call()
    {
        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Game.Adapters.Build",
        (object[] args) =>
        {
            var inputType = (Type)args[0];
            var outputType = (Type)args[1];

        }).Execute();
    }
}