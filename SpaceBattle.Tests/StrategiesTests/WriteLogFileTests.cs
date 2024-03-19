using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;

public class WriteLogFileTests
{
    [Fact]
    public void SuccefulWriteLogFileTest()
    {
        string tempFilePath = Path.GetTempFileName();

        var cmd = new WriteLogFile(tempFilePath, "ExceptionError");
        cmd.Execute();

        var result = File.ReadAllText(tempFilePath);
        var except = "ExceptionError" + "\n";

        Assert.Equal(result, except);
    }
}