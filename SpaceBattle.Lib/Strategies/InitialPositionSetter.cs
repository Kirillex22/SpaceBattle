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
            var stepSize = (int)args[1];
            var objs = (IEnumerable<IUObject>)args[0];

            var start = new Vector(new int[] { 0, 0 });
            var step = new Vector(new int[] { stepSize, 0 });

            var updObjs = IoC.Resolve<IEnumerable<IUObject>>("Game.Generators.Movable.Position", objs, start, step);
            return updObjs;
        }
        ).Execute();
    }
}