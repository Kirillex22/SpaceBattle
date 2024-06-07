using Hwdtech;

namespace SpaceBattle.Lib;

public class RegisterDependenciesCommand : ICommand
{
    private int _gameId;

    public RegisterDependenciesCommand(int gameId)
    {
        _gameId = gameId;
    }

    public void Execute()
    {
        var cmd = IoC.Resolve<ICommand>("Game.Dependencies.Initialization");
        IoC.Resolve<ICommand>("Game.Queue.Push", _gameId, cmd).Execute();
    }
}

