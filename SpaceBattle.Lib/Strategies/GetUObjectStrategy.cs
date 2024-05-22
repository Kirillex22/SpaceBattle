using Hwdtech;
using Hwdtech.Ioc;
using System.Collections;

namespace SpaceBattle.Lib; 

public class GetUObjectStrategy
{
    public object Run(params object[] args)
    {
        int id = (int)args[0];
        var dict = IoC.Resolve<IDictionary<int, IUObject>>("Game.UObject");
        
        if (!dict.TryGetValue(id, out IUObject uobj))
        {
            throw new Exception();
        }

        return uobj;
    }
}

