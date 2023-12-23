using Hwdtech;
using Hwdtech.Ioc;
using System.Collections;

namespace SpaceBattle.Lib;

public class ExceptionHandlerFinder
{
    public ICommand Call(ICommand cmd, Exception exc)
    {
        var exceptionTree = IoC.Resolve<IDictionary>("Game.Struct.ExceptionTree");
        Console.WriteLine(exc.GetType().ToString());
        exceptionTree = (IDictionary?)exceptionTree[cmd.GetType().ToString()];
        Console.WriteLine(exceptionTree[exc.GetType().ToString()].ToString());
        var handler = (ICommand?)exceptionTree[exc.GetType().ToString()];
        return handler;
    }
}

