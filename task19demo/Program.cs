using System;
using System.Linq;
using System.Threading;
using task17;
using task18;

class Program
{
    static void Main()
    {
        Console.WriteLine("Демонстрация работы планировщика");

        var scheduler = new RoundRobinScheduler();
        var serverThread = new task18.ServerThread(scheduler);

        var commands = new TestCommand[5];

        for (int i = 0; i < 5; i++)
        {
            commands[i] = new TestCommand(i + 1);
            serverThread.AddCommand(commands[i]);
        }

        Console.WriteLine("Ожидание выполнения команд 3 раза...");

        bool allDone = false;
        while (!allDone)
        {
            allDone = true;
            foreach (var cmd in commands)
            {
                if (cmd.GetCounter() < 3)
                {
                    allDone = false;
                    break;
                }
                Thread.Sleep(100);
            }
        }

        Console.WriteLine("Все команды выполнились 3 раза. Отправляем HardStop...");
        var hardStop = new HardStop();
        serverThread.AddCommand(hardStop);

        Thread.Sleep(2000);

        Console.WriteLine("Демонстрация завершена");
        Console.WriteLine("Нажмите Enter для выхода...");
        Console.ReadLine();
    }
}