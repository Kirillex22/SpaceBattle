using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;

public class ExceptionHandleStrategy
{
    public ICommand Call(ICommand cmd, Exception e)
    {
        var exceptionTree = IoC.Resolve<IDictionary<object, object>>("Exception.Tree");
        return exceptionTree[cmd.GetType()][e.GetType()](cmd, e);
    }
}