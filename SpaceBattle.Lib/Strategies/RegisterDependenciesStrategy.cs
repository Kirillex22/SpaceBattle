using System;

namespace SpaceBattle.Lib;

public class RegisterDependenciesStrategy
{
    public object Run(params object[] args)
    {
        var gameId = (int)args[0];

        return new RegisterDependenciesCommand(gameId);
    }
}

