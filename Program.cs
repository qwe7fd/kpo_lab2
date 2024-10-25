﻿using System;
using System.Collections.Generic;
using System.IO;

enum State
{
    Winner,
    Loser,
    Playing,
    NotInGame
}

class Player
{
    public string name;
    public int location;
    public State state = State.NotInGame;
    public int distanceTraveled = 0;

    public Player(string name)
    {
        this.name = name;
        this.location = -1;
    }

    public void Move(int steps)
    {
        if (state == State.NotInGame)
        {
            location = steps;
            state = State.Playing;
        }
        else
        {   
            if (location + steps > Game.size) location = location + steps - Game.size;
            else if (location + steps < 1) location = Game.size + (location + steps);
            else location = location + steps;

            distanceTraveled += Math.Abs(steps);
        }
    }
}

enum GameState
{
    Start,
    End
}

class Game
{
    public static string InputFile;
    public static string OutFile;
    public static int size;
    public Player cat;
    public Player mouse;
    public GameState state;
    
    public Game(int size)
    {
        Game.size = size;
        cat = new Player("Cat");
        mouse = new Player("Mouse");
        state = GameState.Start;
    }

    public void Run()
    {
        using StreamReader reader = new StreamReader(InputFile);
        using StreamWriter writer = new StreamWriter(OutFile);
        string line;

        WriteBeginning(writer);
        while (state != GameState.End)
        {
            line = reader.ReadLine();
            if (line == null)
            {
                state = GameState.End;
                break;
            }

            line = line.Trim();
            if (string.IsNullOrEmpty(line))
                continue;

            string[] fragments = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (fragments.Length == 0)
                continue;

            char command = fragments[0][0];        

            if (command == 'P')
            {
                DoPrintCommand(writer);
            }
            else if (fragments.Length > 1 && int.TryParse(fragments[1], out int steps))
            {
                DoMoveCommand(command, steps);
                
                if (GetDistance() == 0)
                {
                    cat.state = State.Winner;
                    mouse.state = State.Loser;
                    state = GameState.End;
                }
            }
        }

        WriteEnd(writer);
    }

    private void DoMoveCommand(char command, int steps)
    {
        switch (command)
        {
            case 'M': mouse.Move(steps); break;
            case 'C': cat.Move(steps); break;
        }
    }

    private void DoPrintCommand(StreamWriter writer)
    {
        string catPosition = cat.state == State.NotInGame ? "??" : cat.location.ToString();
        string mousePosition = mouse.state == State.NotInGame ? "??" : mouse.location.ToString();
        string distance = (cat.state == State.Playing && mouse.state == State.Playing) ? GetDistance().ToString() : "";

        Output(writer, $"{catPosition,3} {mousePosition,5} {distance,9}");
    }
    
    private int GetDistance()
    {
        return Math.Abs(cat.location - mouse.location);
    }

    static void Output(StreamWriter writer, string message)
    {
        Console.WriteLine(message);
        writer.WriteLine(message);
    }

    private void WriteBeginning(StreamWriter writer)
    {
        Output(writer, "Cat and Mouse\n");
        Output(writer, "Cat Mouse  Distance");
        Output(writer, "-------------------");
    }

    private void WriteEnd(StreamWriter writer)
    {
        Output(writer, "-------------------\n");

        Output(writer, $"\nDistance traveled:   Mouse    Cat");
        Output(writer, $"{mouse.distanceTraveled,26}{cat.distanceTraveled,7}\n");

        if (cat.state == State.Winner)
        {
            Output(writer, $"Mouse caught at: {cat.location,2}");
        }
        else
        {
            Output(writer, "Mouse evaded Cat");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Game.InputFile = "3.ChaseData.txt";
        Game.OutFile = "3.PursuitLog.txt";

        using StreamReader reader = new StreamReader(Game.InputFile);
        string size = reader.ReadLine();

        Game game = new Game(int.Parse(size));
        game.Run();
    }
}
