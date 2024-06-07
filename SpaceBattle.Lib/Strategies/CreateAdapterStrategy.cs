using System;
using System.Reflection;
using Hwdtech;

namespace SpaceBattle.Lib;

public class CreateAdapterStrategy
{
    public object Run(params object[] args)
    {
        var uobj = (IUObject)args[0];
        var targetType = (Type)args[1];

        var assemblyDict = IoC.Resolve<IDictionary<KeyValuePair<Type, Type>, Assembly>>("Game.Adapter.Assembly");
        var keyPair = new KeyValuePair<Type, Type>(uobj.GetType(), targetType);

        if (!assemblyDict.TryGetValue(keyPair, out Assembly assembly))
        {
            IoC.Resolve<ICommand>("Game.Adapter.Compile", uobj.GetType(), targetType).Execute();
        }
        var findStrategy = IoC.Resolve<object>("Game.Adapter.Find", uobj, targetType);

        return findStrategy;
    }
}

