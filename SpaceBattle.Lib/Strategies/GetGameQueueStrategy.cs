namespace SpaceBattle.Lib; 

public class GetGameQueueStrategy
{
    public object Run(params object[] args)
    {
        int id = (int)args[0];

        var dict = IoC.Resolve<IDictionary<int, Queue<ICommand>>>("Game.Queue");
        if (!dict.TryGetValue(id, out Queue<Icommand> q))
        {
            throw new Exception();
        }

        return q;
    }
}

