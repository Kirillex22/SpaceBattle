namespace SpaceBattle.Lib;

public class PushQueueStrategy
{
    public object Run(params object[] args)
    {
        var id = (int)args[0];
        var cmd = (ICommand)args[1];

        var q = IoC.Resolve<Queue<ICommand>>("Game.Get.Queue", id);
        q.Enqueue(cmd);

        var action = new ActionCommand(() => {q;});

        return action;
    }
}

