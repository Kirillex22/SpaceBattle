using System;

namespace SpaceBattle.Lib;

public class RegisterCommandsStrategy
{
    public object Run(params object[] args)
    {
        return new RegisterCommandsCommand();
    }
}

