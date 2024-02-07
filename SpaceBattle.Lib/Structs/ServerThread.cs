namespace SpaceBattle.Lib;

public class ServerThread
{
    private Action endAction;
    private bool stop;
    public BlockingCollection<ICommand> ThreadQueue { get; }

    public ServerThread()
    {
        endAction = () => ();
        stop = true;
    }

    public void Start()
    {
        stop = false;

        while (!stop)
        {
            ICommand cmd = ThreadQueue.Take();

            cmd.Execute();
        }

    }
}