using System;

namespace SpaceBattle.Lib;

public class RegisterDependenciesStrategy
{
    public object Run(params object[] args)
    {
        return new RegisterDependenciesCommand();
    }
}

