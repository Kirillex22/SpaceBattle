using Hwdtech;

namespace SpaceBattle.Lib;

public class PositionSetCommand : ICommand
{
    private Guid _id;
    public PositionSetCommand(Guid id) => _id = id;

    public void Execute()
    {
        var obj = IoC.Resolve<IUObject>("Game.IUObject.Container.Get", _id);
        var mvble = IoC.Resolve<IMovable>("Game.Adapters.IMovable", obj);
        var objectPosition = IoC.Resolve<Vector>("Game.Generators.Movable.Position");

        mvble.Position = objectPosition;
    }
}

