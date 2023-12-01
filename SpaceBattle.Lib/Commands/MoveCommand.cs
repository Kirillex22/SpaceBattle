namespace SpaceBattle.Lib;

public class MoveCommand : ICommand
{
    private IMovable _movable;
    public MoveCommand(IMovable movable)
    {
        _movable = movable;
    }
    public void Execute()
    {
        _movable.Position += _movable.Velocity;
    }

}

