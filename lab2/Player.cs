using States;

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