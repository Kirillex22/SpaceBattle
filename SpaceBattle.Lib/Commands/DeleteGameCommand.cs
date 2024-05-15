using System.Collections;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;

public class DeleteGameCommand: ICommand
{
    private int _gameId;

    public DeleteGameCommand(int gameId)
    {
        _gameId = gameId;
    }

    public void Execute()
    {
        gameMap = IoC.Resolve<IDictionary<int, IBridgeCommand>>("Game.Map");
        gameMap[_gameId].Inject(IoC.Resolve<ICommand>("Game.EmptyCommand"));

        scopeMap = IoC.Resolve<IDictionary<int, object>>("Scope.Map");
        scopeMap.Remove(_gameId);
    }
}