using Hwdtech;
namespace SpaceBattle.Lib;

public class FireCommand : ICommand
{
    private IFireable _fireable;

    public FireCommand(IFireable fireable)
    {
        _fireable = fireable;
    }

    public void Execute()
    {
        var torpedo = _fireable.Fire();
        var cmd = IoC.Resolve<ICommand>("Game.Command.StartMove", torpedo);
        cmd.Execute();
    }
}

