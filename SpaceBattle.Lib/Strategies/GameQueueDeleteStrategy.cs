using System.Collections;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;

public class GameQueueDeleteStrategy
{
    public object Run(params object[] args)
    {
        var queueId = (int)args[0];
        var queue = IoC.Resolve<Queue<ICommand>>("Game.Get.Queue");

        return new ActionCommand(() => { queue.Dequeue(); });
    }
}

