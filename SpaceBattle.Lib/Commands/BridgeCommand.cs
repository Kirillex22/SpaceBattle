namespace SpaceBattle.Lib;

public class BridgeCommand: IBridgeCommand
{
    ICommand internalCommand;
    public BridgeCommand(ICommand _internal)
    {
        internalCommand = _internal;
    }

    public void Inject(ICommand other)
    {
        internalCommand = other;
    }

    public void Execute()
    {
        internalCommand.Execute();
    }
}

