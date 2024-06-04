using System;

namespace SpaceBattle.Lib;

public class RegisterDependenciesStrategy : IStrategy
{
    public object Run(params object[] args)
    {
        var gameId = (int)args[0];

        return new RegisterDependenciesCommand(gameId);
    }
}

