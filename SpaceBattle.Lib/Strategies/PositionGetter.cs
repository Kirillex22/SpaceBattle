using Hwdtech;

namespace SpaceBattle.Lib;
public class PositionGetter
{
    private PositionGenerator _posGenerator;

    public PositionGetter() => _posGenerator = new();

    public void Call()
    {
        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Game.Generators.Movable.Position",
        (object[] args) =>
        {
            return _posGenerator.GetNewPosition();
        }
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Game.Generators.Movable.Position.Reset",
        (object[] args) =>
        {
            return new ActionCommand(() => _posGenerator.Reset());
        }
        ).Execute();
    }
}

