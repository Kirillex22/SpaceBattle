using System;
using System.Reflection;
using Hwdtech;

namespace SpaceBattle.Lib;

public class FindAdapterStrategy
{
    public object Run(params object[] args)
    {
        var uobj = (IUObject)args[0];
        var targetType = (Type)args[1];

        var assemblyDict = IoC.Resolve<IDictionary<KeyValuePair<Type, Type>, Assembly>>("Game.Adapter.Assembly");
        var keyPair = new KeyValuePair<Type, Type>(uobj.GetType(), targetType);

        var assembly = assemblyDict[keyPair];
        var type = assembly.GetType(IoC.Resolve<string>("Assembly.Create.Name", targetType))!;

        return Activator.CreateInstance(type, uobj)!;
    }
}

