using System.Collections;
using System.ComponentModel.DataAnnotations;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;
public class GameCreator
{
    public void Call()
    {
        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Game.Initialize.LinearPositionsWithFuel",
        (object[] args) =>
        {
            var count = (int)args[0];
            var objsToPrepare = new ICommand[count];

            var act = () =>
            {
                objsToPrepare.ToList().ForEach(cmd =>
                {
                    var id = Guid.NewGuid();
                    new MacroCommand(new ICommand[]
                    {
                        new CreateObjectCommand(id),
                        new PositionSetCommand(id),
                        new FuelSetCommand(id)
                    }).Execute();
                });
            };

            IoC.Resolve<ICommand>("Game.LinearDisplacer.NewLine").Execute();

            return new ActionCommand(act);
        }).Execute();
    }
}

