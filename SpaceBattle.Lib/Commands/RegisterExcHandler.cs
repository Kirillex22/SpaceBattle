using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;

public class RegisterExcHandler : ICommand
{
    private string _cmd;
    private string _exc;
    private ICommand _handler;

    public RegisterExcHandler(ICommand handler, string cmd = "default", string exc = "default")
    {
        _cmd = cmd;
        _exc = exc;
        _handler = handler;
    } 

    public void Execute()
    {
        var BuildExceptionTree = IoC.Resolve<Dictionary<object, object>>("Game.Get.ExceptionTree");
        var ExceptionTree = BuildExceptionTree;
        ExceptionTree.TryAdd(_cmd, new Dictionary<object, object>());
        ExceptionTree = (Dictionary<object, object>)ExceptionTree[_cmd];
        ExceptionTree.TryAdd(_exc, _handler);
    }
}

