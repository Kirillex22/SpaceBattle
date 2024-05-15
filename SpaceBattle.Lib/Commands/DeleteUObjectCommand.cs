using System.Collections;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;

public class DeleteUObjectCommand
{
    private int _gameId

    public DeleteUObjectCommand(int gameId)
    {
        _gameId = gameId;
    }
    public void Execute()
    {
        var uobjDict = IoC.Resolve<IDictionary<int, IUObject>>("Game.UObject");
        if (!uobjDict.TryGetValue(_gameId, out IUObject uobj))
        {
            throw new Exception("The object with the specified ID wasn't found");
        }

        uobjDict.Remove(_gameId);
    }
}

