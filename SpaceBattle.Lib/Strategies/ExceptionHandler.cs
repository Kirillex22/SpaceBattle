using Hwdtech;

namespace SpaceBattle.Lib;

public class ExceptionHandler
{
    private ExceptionHandlerFinder _finder;

    public ExceptionHandler()
    {
        _finder = new ExceptionHandlerFinder();
    }

    public void Call()
    {
        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Game.ExceptionHandler.Handle",
        (object[] args) =>
        {
            var cmd = (ICommand)args[0];
            var exc = (Exception)args[1];
            return _finder.Call(cmd, exc);

        }).Execute();
    }
}

