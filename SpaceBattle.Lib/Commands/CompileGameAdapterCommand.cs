using System.Reflection;
using Hwdtech;

namespace SpaceBattle.Lib;

public class CompileGameAdapterCommand : ICommand
{
    private Type _objectType;
    private Type _targetType;

    public CompileGameAdapterCommand(Type objectType, Type targetType)
    {
        _objectType = objectType;
        _targetType = targetType;
    }

    public void Execute()
    {
        var adapter = IoC.Resolve<string>("Game.Adapter.Code", _objectType, _targetType);
        var assembly = IoC.Resolve<Assembly>("Compile", adapter);

        var assemblyDict = IoC.Resolve<IDictionary<KeyValuePair<Type, Type>, Assembly>>("Game.Adapter.Assembly");
        var keyPair = new KeyValuePair<Type, Type>(_objectType, _targetType);

        assemblyDict[keyPair] = assembly;
    }
}

