using System.Collections;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;

public class DeleteUObjectCommand : ICommand
{
    private int _gameId;

    public DeleteUObjectCommand(int gameId)
    {
        _gameId = gameId;
    }
    public void Execute()
    {
        var uobjDict = IoC.Resolve<IDictionary<int, IUObject>>("Game.UObject");
        uobjDict.Remove(_gameId);
    }
}

