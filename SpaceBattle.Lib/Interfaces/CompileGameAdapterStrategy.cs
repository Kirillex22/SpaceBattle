namespace SpaceBattle.Lib;

public class CompileGameAdapterStrategy
{
    public object Run(params object[] args)
    {
        var objectType = (Type)args[0];
        var targetType  = (Type)args[1];

        return new CompileGameAdapterCommand(objectType, targetType);
    }
}

