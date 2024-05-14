namespace SpaceBattle.Lib;

public class ActionCommand : ICommand
{
    private Action _act;
    public ActionCommand(Action act)
    {
        _act = act;
    }

    public void Execute()
    {
        _act();
    }
}

