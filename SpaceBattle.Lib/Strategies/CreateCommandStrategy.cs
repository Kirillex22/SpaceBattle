using Hwdtech;
using Hwdtech.Ioc;
using System.Collections;

namespace SpaceBattle.Lib;

public class CreateCommandStrategy
{
    public object Run(params object[] args)
    {
        IMessage message = (IMessage)args[0];

        var uobj = IoC.Resolve<IUObject>("Game.Get.UObject", message.ItemId);

        message.Properties.ToList().ForEach(i => IoC.Resolve<ICommand>("Game.Command.SetProperties", uobj, i.Key, i.Value).Execute());

        return IoC.Resolve<ICommand>("Game.Command" + message.CommandType, uobj);
    }
}

