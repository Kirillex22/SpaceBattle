using Hwdtech;

namespace SpaceBattle.Lib;
public class LinearDisplacer
{
    private Vector _start;
    private Vector _stepX;
    private Vector _stepY;

    public LinearDisplacer()
    {
        _start = new Vector(new int[] { 0, 0 });
        _stepX = new Vector(new int[] { 1, 0 });
        _stepY = new Vector(new int[] { 0, 1 });
    }
    public void Call()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.LinearDisplacer.NewLine", (object[] args) => new ActionCommand(() =>
        {
            IoC.Resolve<ICommand>("Game.Generators.Movable.Position.Reset").Execute();
            _start += _stepX;
        }
        )).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Initialize.Movable.StartPosition", (object[] args) => _start).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Initialize.Movable.Position", (object[] args) =>
        {
            var previousPos = (Vector)args[0];
            return previousPos + _stepY;
        }).Execute();
    }
}

