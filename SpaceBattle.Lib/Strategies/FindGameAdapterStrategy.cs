using System.Reflection;
using Hwdtech;

namespace SpaceGame.Lib;

public class FindGameAdapterStrategy
{
    public object Run(params object[] args)
    {
        var uobj = (IUObject)args[0];
        var targetType = (Type)args[1];

        var assemblyMap = IoC.Resolve<IDictionary<KeyValuePair<Type, Type>, Assembly>>("Game.Adapter.Assembly.Map");
        var pair = new KeyValuePair<Type, Type>(uobj.GetType(), targetType);
        var assembly = assemblyMap[pair];
        var type = assembly.GetType(IoC.Resolve<string>("Game.Adapter.Name.Create", targetType))!;

        return Activator.CreateInstance(type, uobj)!;
    }
}

