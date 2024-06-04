using System;

namespace SpaceBattle.Lib;

public class RegisterCommandsStrategy : IStrategy
{
    public object Run(params object[] args)
    {
        return new RegisterCommandsCommand();
    }
}

