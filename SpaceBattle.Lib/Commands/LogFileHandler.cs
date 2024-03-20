namespace SpaceBattle.Lib;

public class LogFileHandler : IHandler
{
    private string _path;
    private string _message;

    public LogFileHandler(string path, string message)
    {
        _path = path;
        _message = message;
    }

    public void Handle()
    {
        File.AppendAllText(_path, _message+"\n");
    }
}

