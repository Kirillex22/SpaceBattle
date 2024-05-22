using Hwdtech;
using Hwdtech.Ioc;
using System.Collections;

namespace SpaceBattle.Lib; 

public class GetGameQueueStrategy
{
    public object Run(params object[] args)
    {
        int id = (int)args[0];

        var dict = IoC.Resolve<IDictionary<int, Queue<SpaceBattle.Lib.ICommand>>>("Game.Queue.Dict");
        if (!dict.TryGetValue(id, out Queue<SpaceBattle.Lib.ICommand> q))
        {
            throw new Exception();
        }

        return q;
    }
}

