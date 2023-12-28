using Hwdtech;
using Hwdtech.Ioc;
using System.Collections;

namespace SpaceBattle.Lib;

public class ExceptionHandlerFinder
{
    public ICommand Call(ICommand cmd, Exception exc)
    {
        var searcher = (IDictionary currentTree, object parameter) =>
        {
            return currentTree.Contains(parameter) ? currentTree[parameter] : currentTree["default"];
        };

        var exceptionTree = IoC.Resolve<IDictionary>("Game.Struct.ExceptionTree");
        exceptionTree = (IDictionary)searcher(exceptionTree, cmd.GetType().ToString());
        var handler = (ICommand)searcher(exceptionTree, exc.GetType().ToString());

        return handler;
    }
}

