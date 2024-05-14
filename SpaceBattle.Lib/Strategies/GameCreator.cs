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
                var playerFirstObjs = IoC.Resolve<IEnumerable<IUObject>>("Game.Generators.IUObject", count / 2); //корабли 1 игрока
                var playerSecondObjs = IoC.Resolve<IEnumerable<IUObject>>("Game.Generators.IUObject", count / 2); //корабли 2 игрока

                playerFirstObjs = IoC.Resolve<IEnumerable<IUObject>>("Game.Initialize.StartPositions", playerFirstObjs, 0, 1);
                playerFirstObjs = IoC.Resolve<IEnumerable<IUObject>>("Game.Initialize.StartFuelCapacity", playerFirstObjs, 100);

                playerSecondObjs = IoC.Resolve<IEnumerable<IUObject>>("Game.Initialize.StartPositions", playerSecondObjs, 1, 1);
                playerSecondObjs = IoC.Resolve<IEnumerable<IUObject>>("Game.Initialize.StartFuelCapacity", playerSecondObjs, 100);
            };

            return new ActionCommand(act);
        }
        ).Execute();
    }
}



