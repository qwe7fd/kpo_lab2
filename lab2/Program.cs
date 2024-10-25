using System;
using System.Collections.Generic;
using System.IO;

using States;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Input a number of ChaseData file: ");
        int number = int.Parse(Console.ReadLine());

        Game.InputFile = "C:\\Users\\porva\\Desktop\\edu\\.prog\\.cs\\lab2\\" + number + ".ChaseData.txt";
        Game.OutFile = "C:\\Users\\porva\\Desktop\\edu\\.prog\\.cs\\lab2\\" + number + ".PursuitLog.txt";

        using StreamReader reader = new StreamReader(Game.InputFile);
        string size = reader.ReadLine();

        Game game = new Game(int.Parse(size));
        game.Run();
    }
}