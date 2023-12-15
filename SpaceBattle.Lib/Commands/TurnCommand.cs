namespace SpaceBattle.Lib;

public class Turn : ICommand
{
    private ITurnable _turnable;
    public Turn(ITurnable turnable)
    {
        _turnable = turnable;
    }
    public void Execute()
    {
        _turnable.Angle += _turnable.AngleVelocity;
    }
}

