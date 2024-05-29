using System;
using Hwdtech;

namespace SpaceBattle.Lib;

public class CompileGameAdapterCommand : ICommand
{
    public Type _objectType;
    public Type _targetType;

    public CompileGameAdapterCommand(Type objectType, Type targetType)
    {
        _objectType = objectType;
        _targetType = targetType;
    }
    
    public void Execute()
    {
        var codeAdapter = IoC.Resolve<string>("Game.Code.Adapter", _objectType, _targetType);
        var assembly = IoC.Resolve<Assembly>("Compile", codeAdapter);
        var assemblyMap = IoC.Resolve<IDictionary<KeyValuePair<Type, Type>, Assembly>>("Game.Adapter.Assembly.Map");
        var pair = new KeyValuePair<Type, Type>(_objectType, _targetType);

        assemblyMap[pair] = assembly;
    }
}

