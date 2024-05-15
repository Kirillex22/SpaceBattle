using System.Collections;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;

public class DeleteUObjectStrategy
{
    public object Run(params object[] args)
    {
        var uobjId = (int)args[0];

        return new DeleteUObjectCommand(uobjId);
    }
}

