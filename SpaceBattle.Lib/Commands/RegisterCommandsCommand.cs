using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;

public class RegisterCommandsCommand : ICommand
{
    public void Execute()
    {
        var dependencies = IoC.Resolve<IDictionary<string, IStrategy>>("Game.Dependencies.Get");

        dependencies.ToList().ForEach(i =>
        {
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command." + i.Key, (object[] args) => i.Value.Run(args)).Execute();
        });
    }
}

