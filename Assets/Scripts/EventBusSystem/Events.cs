public interface IEvent
{
}


public struct TestEvnet : IEvent
{
}

public struct PlayerEvent : IEvent
{
    public int health;
    public int score;
}

public struct HealthEvent : IEvent
{
    public int hurts;
}

public struct GameOverEvent : IEvent
{
}

public struct PauseEvent : IEvent
{
    public bool isPaused;
}

public struct ScoreEvent : IEvent
{
    public int ScoreGained;
}