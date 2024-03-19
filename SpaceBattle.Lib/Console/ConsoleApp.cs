using System;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;

class ConsoleApp
{
    static void Main(int count)
    {
        Console.WriteLine("Идет запуск сервера...");
        IoC.Resolve<ICommand>("Server.Start", count).Execute();
        Console.WriteLine("Сервер успешно запущен");
        Console.ReadKey();
        Console.WriteLine("Идет остановка сервера...");
        IoC.Resolve<ICommand>("Server.Stop").Execute();
        Console.WriteLine("Сервер успешно остановлен");
    }
}

