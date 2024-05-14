using System.Collections;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;
public class GameCreator
{
    public void Call()
    {
        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Game.SetInitialState",
        (object[] args) =>
        {
            var count = (int)args[0];

            var act = () =>
            {
                var objs = IoC.Resolve<IEnumerable<IUObject>>("Game.Generators.IUObject", count);
                objs = IoC.Resolve<IEnumerable<IUObject>>("Game.Initialize.StartPositions", objs, 1);
                objs = IoC.Resolve<IEnumerable<IUObject>>("Game.Initialize.StartFuelCapacity", objs, 100);
            };

            return new ActionCommand(act);
        }
        ).Execute();
    }
}



