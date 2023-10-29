namespace SpaceBattle.Lib;
public class Turn : ICommand
{
    private ITurnable turnable;
    public Turn(ITurnable turnable)
    {
        this.turnable = turnable;
    }
    public void Execute()
    {
        turnable.angle += turnable.angle_velocity;
    }
}