using Hwdtech;

namespace SpaceBattle.Lib;

public class RegisterCommandsCommand
{
    public void Execute()
    {
        var dependencies = IoC.Resolve<IDictionary<string, IStrategy>>("Game.Dependencies.Get");

        dependencies.ToList().ForEach( i => {IoC.Resolve<ICommand>("Game.Command." + i.Key).Execute()} );
    }
}

