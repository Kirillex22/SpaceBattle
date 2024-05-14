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
            var startX = (int)args[1];
            var stepY = (int)args[2];
            var stepX = (int)args[3];
            var initCapacity = (int)args[4];

            var act = () =>
            {
                var playerFirstObjs = IoC.Resolve<List<IUObject>>("Game.Generators.IUObject", count / 2); //корабли 1 игрока
                var playerSecondObjs = IoC.Resolve<List<IUObject>>("Game.Generators.IUObject", count / 2); //корабли 2 игрока

                playerFirstObjs = IoC.Resolve<List<IUObject>>("Game.Initialize.StartPositions", playerFirstObjs, startX, stepY);
                playerFirstObjs = IoC.Resolve<List<IUObject>>("Game.Initialize.StartFuelCapacity", playerFirstObjs, initCapacity);

                playerSecondObjs = IoC.Resolve<List<IUObject>>("Game.Initialize.StartPositions", playerSecondObjs, startX + stepX, stepY);
                playerSecondObjs = IoC.Resolve<List<IUObject>>("Game.Initialize.StartFuelCapacity", playerSecondObjs, initCapacity);
            };

            return new ActionCommand(act);
        }
        ).Execute();
    }
}



