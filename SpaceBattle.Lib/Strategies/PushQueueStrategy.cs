using Hwdtech;
using Hwdtech.Ioc;
using System.Collections;

namespace SpaceBattle.Lib;

public class PushQueueStrategy
{
    public object Run(params object[] args)
    {
        var id = (int)args[0];
        var cmd = (ICommand)args[1];

        var q = IoC.Resolve<Queue<ICommand>>("Game.Get.Queue", id);
        var action = new ActionCommand(() => {q.Enqueue(cmd);});

        return action;
    }
}

