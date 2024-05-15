using System.Collections;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;

public class DeleteGameStrategy
{
    public object Run(params object[] args)
    {
        int gameId = (int)args[0];
        return new DeleteGameCommand(gameId);
    }
}

