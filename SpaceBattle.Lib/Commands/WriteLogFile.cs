namespace SpaceBattle.Lib;

public class WriteLogFile : ICommand
{
    private string _path;
    private string _message;

    public WriteLogFile(string path, string message)
    {
        _path = path;
        _message = message;
    }

    public void Execute()
    {
        File.AppendAllText(_path, _message+"\n");
    }
}

