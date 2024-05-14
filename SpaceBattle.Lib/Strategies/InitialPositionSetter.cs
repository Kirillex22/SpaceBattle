using System.Collections;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;
public class InitialPositionSetter
{
    private PositionGenerator _posGen = new();

    public InitialPositionSetter() => _posGen.Call();

    public void Call()
    {
        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Game.Initialize.StartPositions",
        (object[] args) =>
        {
            var startPointX = (int)args[1];
            var stepSizeY = (int)args[2];
            var objs = (List<IUObject>)args[0];

            var start = new Vector(new int[] { startPointX, 0 });
            var step = new Vector(new int[] { 0, stepSizeY });
            var updObjs = IoC.Resolve<List<IUObject>>("Game.Generators.Movable.Position", objs, start, step);
            return updObjs;
        }
        ).Execute();
    }
}

