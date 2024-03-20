using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;

public class LogFileHandlerTests
{
    [Fact]
    public void SuccefulLogFileHandlerTest()
    {
        string tempFilePath = Path.GetTempFileName();

        var handle = new LogFileHandler(tempFilePath, "ExceptionError");
        handle.Handle();

        var result = File.ReadAllText(tempFilePath);
        var except = "ExceptionError" + "\n";

        Assert.Equal(result, except);
    }
}